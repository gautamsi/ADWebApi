using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTfulAD.APIService
{
    public enum LDAPFilterOperator
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
    }

    public class LDAPFilterBuilder
    {
        int closingCount = 0;
        List<string> conditions = new List<string>();
        Stack<string> lastOperator = new Stack<string>();

        public LDAPFilterBuilder()
        {
            lastOperator.Push(null);
        }
        public string Filter { get; private set; }
        public string Build()
        {
            while (closingCount > 0)
            {
                flush();
            }
            Filter = ToString();
            return Filter;
        }

        public void Clear()
        {
            this.Filter = "";
            this.conditions.Clear();
            this.closingCount = 0;
        }

        public LDAPFilterBuilder Condition(string property, string value, LDAPFilterOperator _operator = LDAPFilterOperator.Equal)
        {
            if (_operator == LDAPFilterOperator.NotEqual)
            {
                conditions.Add("(!(" + property + _operator.ToOperatorString() + value + "))");
            }
            else
            {
                conditions.Add("(" + property + _operator.ToOperatorString() + value + ")");
            }
            return this;
        }

        public LDAPFilterBuilder StartAnd()
        {
            PositionOperator("(&");
            return this;
        }
        public LDAPFilterBuilder And(string property, string value, LDAPFilterOperator condition = LDAPFilterOperator.Equal)
        {
            PositionOperator("(&");
            return Condition(property, value, condition);
        }
        public LDAPFilterBuilder StartOr()
        {
            PositionOperator("(|");
            return this;
        }
        public LDAPFilterBuilder Or(string property, string value, LDAPFilterOperator condition = LDAPFilterOperator.Equal)
        {
            PositionOperator("(|");
            return Condition(property, value, condition);
        }
        public LDAPFilterBuilder CloseCondition()
        {
            flush();
            return this;
        }
        private void flush()
        {
            if (closingCount > 0)
            {
                conditions.Add(")");
                closingCount--;
                if (lastOperator.Count > 0)
                    lastOperator.Pop();
            }
        }



        private void PositionOperator(string value)
        {
            if (lastOperator.Peek() == value)
                return;

            if (conditions.Count == 0)
            {
                conditions.Add(value);
            }
            else if (conditions[0].StartsWith("(&") || conditions[0].StartsWith("(|"))
            {
                if (conditions[-1] == ")")
                {
                    conditions.Add(value);
                }
                else {
                    conditions.Insert(conditions.Count - 1, value);
                }
            }
            else
            {
                conditions.Insert(0, value);
            }
            closingCount++;
            lastOperator.Push(value);
        }

        public override string ToString()
        {
            return string.Join("", conditions);
        }
    }

    class LDAPFilterConditionBuilder : LDAPFilterBuilder
    {
        internal LDAPFilterConditionBuilder() : base()
        {

        }

    }
}
