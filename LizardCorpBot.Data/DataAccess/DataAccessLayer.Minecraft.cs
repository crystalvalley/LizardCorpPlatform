namespace LizardCorpBot.Data.DataAccess
{
    using LizardCorpBot.Data.Model;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// partial클래스로 분할, 마인크래프트 관련 데이터 처리.
    /// </summary>
    public partial class DataAccessLayer
    {
        /// <summary>
        /// 마인크래프트 유저 정보 추가.
        /// 만약에 유저 정보는 있고 id만 추가할 경우 갱신함.
        /// </summary>
        /// <param name="name">마인크래프트 캐릭터 이름</param>
        /// <param name="userid">디스코드 유저 id.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과 반환.</returns>
        public async Task AddMinecraftUser(string name, ulong? userid = null)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var user = await GetMinecraftUserAsync(name);
            if (user is null)
            {
                await context.MinecraftUsers.AddAsync(new MinecraftUser()
                {
                    IsPlaying = false,
                    Name = name,
                    LastJoined = DateTime.UtcNow,
                    PlayTime = 0,
                    UserId = userid,
                });
            }
            else
            {
                user.UserId = userid;
            }

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// 마인크래프트 캐릭터명 변경.
        /// </summary>
        /// <param name="oldName">마인크래프트 변경 전 캐릭터 이름.</param>
        /// <param name="newName">마인크래프트 변경 후 캐릭터 이름.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과 반환.</returns>
        public async Task ChanageName(string oldName, string newName)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var user = await GetMinecraftUserAsync(oldName);

            // 유저가 없으면 새로 등록
            if (user is null) await AddMinecraftUser(newName);
            else
            {
                user.Name = newName;
            }

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// 마인크래프트 서버 접속시.
        /// </summary>
        /// <param name="name">마인크래프트 캐릭터 이름.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과 반환.</returns>
        public async Task JoinMinecraftServer(string name)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var user = await GetMinecraftUserAsync(name);

            // 등록되지 않은 유저라면 등록 후 재실행
            if (user is null)
            {
                await AddMinecraftUser(name);
                await JoinMinecraftServer(name);
                return;
            }

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// 마인크래프트 서버 접속 종료시.
        /// </summary>
        /// <param name="name">마인크래프트 캐릭터 이름.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과 반환.</returns>
        public async Task LeftMinecraft(string name)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var user = await GetMinecraftUserAsync(name);

            // 등록되지 않은 유저라면 등록 후 재실행
            if (user is null)
            {
                await AddMinecraftUser(name);
                await LeftMinecraft(name);
                return;
            }

            // 유저 정보가 없거나, 유저가 플레이 중이 아니었다거나, 유저의 최종 접속 시간이 없을 경우
            // 일반적으로는 없을 테지만 디스코드봇 재기동 중에 접속 했다거나, 유저가 생성되었을 경우 가능함.
            // 이 경우에는 플레이타임 집계 무시.
            if (user is null || !user.IsPlaying || user.LastJoined is null) return;
            user.IsPlaying = false;

            long playTime = DateTime.UtcNow.Ticks - ((DateTime)user.LastJoined).Ticks;
            user.PlayTime += playTime;
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// 마인크래프트 유저 정보 참조.
        /// </summary>
        /// <param name="uid">디스코드 유저 id.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과로 마인크래프트 유저를 반환.</returns>
        public async Task<MinecraftUser?> GetMinecraftUserAsync(ulong uid)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            return await context.MinecraftUsers.Where(u => u.UserId == uid).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 마인크래프트 유저 정보 참조.
        /// </summary>
        /// <param name="name">마인크래프트 캐릭터 이름.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과로 마인크래프트 유저를 반환.</returns>
        public async Task<MinecraftUser?> GetMinecraftUserAsync(string name)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            return await context.MinecraftUsers.Where(u => u.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 접속중인 유저리스트 참조.
        /// 디스코드 유저 id를 지정할 경우 한 명만 찾아냄.
        /// </summary>
        /// <param name="uid">디스코드 유저 id.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과로 마인크래프트 유저리스트를 반환.</returns>
        public async Task<List<MinecraftUser>> GetOnelineUserAsync(ulong? uid = null)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var query = context.MinecraftUsers.Where(u => u.IsPlaying);
            if (uid != null)
            {
                query = query.Where(u => u.UserId == uid);
            }

            return await query.ToListAsync();
        }
    }
}
