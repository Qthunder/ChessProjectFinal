using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChessProjectFinal.Common
{
    public abstract class BasePropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Indicates that a property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event for multiple properties
        /// </summary>
        /// <param name="propertyNames">The names of the properties that changed</param>
        public virtual void RaisePropertyChanged(params string[] propertyNames)
        {
            if (this.PropertyChanged != null)
            {
                foreach (string propName in propertyNames)
                {
                    RaisePropertyChangedInternal(propName);
                }
            }
        }

        /// <summary>
        /// Raises the property changed event
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        public virtual void RaisePropertyChanged(string propertyName)
        {
            RaisePropertyChangedInternal(propertyName);
        }

        /// <summary>
        /// Raises the property change event based upon an expression
        /// </summary>
        /// <typeparam name="T">The type of the property that changed</typeparam>
        /// <param name="property">The expression returning the property that changed</param>
        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            string propertyName = ReflectionHelper.ExtractPropertyName(property);
            RaisePropertyChangedInternal(propertyName);
        }

        /// <summary>
        /// Raises the property changed event
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        private void RaisePropertyChangedInternal(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
