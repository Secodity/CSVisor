using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CSVisor.Core.BaseClasses
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly ConcurrentDictionary<string, object> _properties;

        public ViewModelBase()
        {
            _properties = new ConcurrentDictionary<string, object>();
        }


        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Get
        protected T Get<T>(Expression<Func<T>> expression)
        {
            return Get<T>(__GetPropertyName(expression));
        }
        protected T Get<T>(Expression<Func<T>> expression, T defaultValue)
        {
            return Get(__GetPropertyName(expression), defaultValue);
        }

        protected T Get<T>(T defaultValue, [CallerMemberName] string propertyName = null)
        {
            return Get(propertyName, defaultValue);
        }
        protected T Get<T>([CallerMemberName] string name = null)
        {
            return Get(name, default(T));
        }

        protected T Get<T>(string name, T defaultValue)
        {
            return GetValueByName<T>(name, defaultValue);
        }

        protected T GetValueByName<T>(String name, T defaultValue)
        {

            if (_properties.TryGetValue(name, out var val))
                return (T)val;

            return defaultValue;
        }
        #endregion

        #region [Set]

        protected void Set<T>(Expression<Func<T>> expression, T value)
        {
            Set(__GetPropertyName(expression), value);
        }

        protected void Set<T>(T value, [CallerMemberName] string propertyName = "")
        {
            Set(propertyName, value);
        }

        public void Set<T>(string name, T value)
        {
            if (_properties.TryGetValue(name, out var val))
            {
                if (val == null && value == null)
                    return;

                if (val != null && val.Equals(value))
                    return;
            }
            _properties[name] = value;
            OnPropertyChanged(name);
        }
        #endregion

        private string __GetPropertyName<T>(Expression<Func<T>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException($"{nameof(expression)} must be a property {nameof(expression)}");

            return memberExpression.Member.Name;
        }

    }
}
