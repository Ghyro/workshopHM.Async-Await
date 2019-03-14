using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskLibrary.Enums;
using TaskLibrary.Interfaces;

namespace TaskLibrary.Services
{
    public class Constraint : IConstrains
    {
        private readonly Constraints _constraint;

        public Constraint(Constraints constraints)
        {
            this._constraint = constraints;
        }

        public bool IsMatch(Uri currentUri, Uri secondUri)
        {
            if (_constraint == Constraints.WithoutConstraints)
            {
                return true;
            }
            else if (_constraint == Constraints.CurrentDomainOnly && currentUri.DnsSafeHost == secondUri.DnsSafeHost)
            {
                return true;
            }
            else if (_constraint == Constraints.DescendingPagesOnly && secondUri.IsBaseOf(currentUri))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
