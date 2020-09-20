﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hb.MarsRover.Domain.Types
{
    public abstract class Enumeration : Entity, IComparable
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

        public int EnumId { get; private set; }

        protected Enumeration()
        { }

        protected Enumeration(int enumId, string name)
        {
            EnumId = enumId;
            Name = name;
        }

        protected Enumeration(int enumId, string code, string name)
        {
            EnumId = enumId;
            Name = name;
            Code = code;
        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.EnumId - secondValue.EnumId);
            return absoluteDifference;
        }

        public static T FromValue<T>(int value) where T : Enumeration
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.EnumId == value);
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
            return matchingItem;
        }
        public static T FromCode<T>(string code) where T : Enumeration
        {
            var matchingItem = Parse<T, string>(code, "code", item => item.Code == code);
            return matchingItem;
        }

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

            return matchingItem;
        }

        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
    }
}