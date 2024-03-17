namespace LizardCorpPlatform.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Security.AccessControl;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// VO.
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// VO의 Equals메소드 override.
        /// </summary>
        /// <param name="obj">비교할 대상.</param>
        /// <returns>비교 결과 동일하면 true.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType()) return false;
            var other = obj as ValueObject;
            return base.Equals(other);
        }

        /// <summary>
        /// VO의 == 연산자 확장.
        /// </summary>
        /// <param name="arg1">비교대상1.</param>
        /// <param name="arg2">비교대상2.</param>
        /// <returns>비교 결과 동일하면 true.</returns>
        protected static bool EqualOperator(ValueObject arg1, ValueObject arg2)
        {
            if (arg1 is null ^ arg2 is null) return false;
            return arg1?.Equals(arg2!) != false;
        }

        /// <summary>
        /// VO의 != 연산자 확장.
        /// </summary>
        /// <param name="arg1">비교대상1.</param>
        /// <param name="arg2">비교대상2.</param>
        /// <returns>비교 결과 동일하면 true.</returns>
        protected static bool NotEqualOperator(ValueObject arg1, ValueObject arg2)
        {
            return !EqualOperator(arg1, arg2);
        }
    }
}
