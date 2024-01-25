using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Runtime.CompilerServices;

namespace Prism.Mvvm;

#nullable disable

public abstract class BindableObject : BindableBase, INotifyPropertyChanging
{
    public event PropertyChangingEventHandler PropertyChanging;

    protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

        RaisePropertyChanging(propertyName);
        storage = value;
        RaisePropertyChanged(propertyName);

        return true;
    }

    protected override bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

        RaisePropertyChanging(propertyName);
        storage = value;
        onChanged?.Invoke();
        RaisePropertyChanged(propertyName);

        return true;
    }

    protected virtual bool SetProperty<T>(ref T storage, T value, Action<T, T> onChanging, Action<T, T> onChanged, [CallerMemberName] string propertyName = null!)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

        RaisePropertyChanging(propertyName);

        onChanging?.Invoke(storage, value);
        var old = storage;
        storage = value;
        onChanged?.Invoke(old, storage);

        RaisePropertyChanged(propertyName);
        return true;
    }

    protected virtual bool SetProperty<T>(ref T storage, T value, Action<T> onChanging1, Action<T, T> onChanging2, Action<T, T> onChanged1, Action<T> onChanged2, [CallerMemberName] string propertyName = null!)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

        RaisePropertyChanging(propertyName);

        onChanging1?.Invoke(storage);
        onChanging2?.Invoke(storage, value);
        var old = storage;
        storage = value;
        onChanged1?.Invoke(old, storage);
        onChanged2?.Invoke(storage);

        RaisePropertyChanged(propertyName);
        return true;
    }

    protected void RaisePropertyChanging([CallerMemberName] string propertyName = null)
    {
        OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
    }

    protected virtual void OnPropertyChanging(PropertyChangingEventArgs args)
    {
        PropertyChanging?.Invoke(this, args);
    }
}

#nullable enable
