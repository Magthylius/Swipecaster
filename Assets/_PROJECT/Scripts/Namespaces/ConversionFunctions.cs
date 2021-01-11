using System;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using Component = UnityEngine.Component;

namespace ConversionFunctions
{
    public static class FindObject
    {
        public static Unit AsUnit(this GameObject gameObject) => gameObject != null ? gameObject.GetComponent<Unit>() : null;
        public static Unit AsUnit<T>(this T tee) where T : Component => tee != null ? tee.GetComponent<Unit>() : null;
        public static Summon AsSummon(this GameObject gameObject) => gameObject != null ? gameObject.GetComponent<Summon>() : null;
        public static Summon AsSummon<T>(this T tee) where T : Component => tee != null ? tee.GetComponent<Summon>() : null;

        public static T AsType<T>(this GameObject gameObject) where T : Component => gameObject != null ? gameObject.GetComponent<T>() : null;
        public static T AsType<T>(this T tee) where T : Component => tee != null ? tee.GetComponent<T>() : null;

        public static float AsFloat(this int number) => number;
        public static int AsInt<T>(this T item) where T : Enum => Convert.ToInt32(item);
        public static int AsInt(this float item) => Mathf.RoundToInt(item);
        public static int AsInt(this bool item) => Convert.ToInt32(item);
        public static bool AsBool(this int item) => Convert.ToBoolean(item);
    }

    public static class TenshiStrings
    {
        public static string NameOfClass<T>(this T type) where T : class => TypeDescriptor.GetClassName(type.GetType());

        public static string AddSpacesBeforeCapitalLetters(this string text, bool hasAcronym)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            Append(text[0]);
            for(int i = 1; i < text.Length; i++)
            {
                if (CanAddSpace()) Append(' ');
                Append(text[i]);

                #region Local Shorthands

                bool CanAddSpace() => CurrentIsUpperCase() && (PreviousIsLowerCase() || NotPartOfAcronym());

                bool PreviousIsLowerCase() => PreviousCharNotASpace() && !PreviousIsUpperCase();
                bool NotPartOfAcronym() => hasAcronym &&
                                           PreviousIsUpperCase() &&
                                           CurrentIsNotLastElement() &&
                                           !NextIsUpperCase();

                bool CurrentIsNotLastElement() => i < text.Length - 1;
                bool PreviousCharNotASpace() => text[i - 1] != ' ';
                bool CurrentIsUpperCase() => char.IsUpper(text[i]);
                bool PreviousIsUpperCase() => char.IsUpper(text[i - 1]);
                bool NextIsUpperCase() => char.IsUpper(text[i + 1]);

                #endregion
            }
            return newText.ToString();
            void Append(char c) => newText.Append(c);
        }
    }
}