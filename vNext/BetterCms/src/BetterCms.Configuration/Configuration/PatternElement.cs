using System;
using System.Configuration;

namespace BetterCms.Configuration
{
    public class PatternElement : ConfigurationElement
    {
        private const string ExpressionAttribute = "expression";
        private const string NegateAttribute = "negate";
        private const string DescriptionAttribute = "description";
        private const string IgnoreCaseAttribute = "ignoreCase";

        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        /// <value>
        /// The expression.
        /// </value>
        [ConfigurationProperty(ExpressionAttribute, DefaultValue = "", IsRequired = true)]
        public string Expression
        {
            get { return Convert.ToString(this[ExpressionAttribute]); }
            set { this[ExpressionAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PatternElement"/> should be negated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if negate; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(NegateAttribute, DefaultValue = false, IsRequired = false)]
        public bool Negate
        {
            get { return Convert.ToBoolean(this[NegateAttribute]); }
            set { this[NegateAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [ConfigurationProperty(DescriptionAttribute, DefaultValue = "", IsRequired = false)]
        public string Description
        {
            get { return Convert.ToString(this[DescriptionAttribute]); }
            set { this[DescriptionAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PatternElement"/> should be checked by ignoring case.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ignore case; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(IgnoreCaseAttribute, DefaultValue = false, IsRequired = false)]
        public bool IgnoreCase
        {
            get { return Convert.ToBoolean(this[IgnoreCaseAttribute]); }
            set { this[IgnoreCaseAttribute] = value; }
        }
    }
}