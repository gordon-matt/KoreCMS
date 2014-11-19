using System;

namespace Kore.Web.Mvc.RoboUI
{
    public abstract class RoboUIGridColumn
    {
        internal string HeaderText { get; set; }

        public string PropertyName { get; set; }

        public Type PropertyType { get; set; }

        public string DisplayFormat { get; set; }

        internal int? Width { get; set; }

        public string Align { get; set; }

        public string CssClass { get; set; }

        public abstract string BuildHtml(object item);

        public abstract object GetValue(object item);
    }

    public class RoboUIGridColumn<TModel> : RoboUIGridColumn
    {
        //TODO: this crap needs to be changed.. it should not be hard coded like this..
        private const string TrueIcon =
            "data:image/gif;base64,R0lGODlhEAAQAOYAAARsE5zgd0/AJtfsyi6wFoS1hACZALLWsmbIQx+oCwCICDmMO7/ov/r2/Pnq+gBtJjq1HIXMeqfJqu/37xaFF9Hh0XnTTCydJ2bMMw2jAKflg5Xcd0CmQRNzGVC+O+rm7EW7IgByMojYXi6gKgCFEiWjF9/v2wB9IRyUHGzGU4zFjDq1IQBmMweeAPXw9gCND4i/jiqqIBupCH7TWQB8Kf///1rFLEe8ISetC+nz3C+oJBF6GCStEUK1IbjfuIa7hjGoKB6RHXPKVgBsOpvde1TDKTqUOkKrQo/LjwiODk3AJwuNCCmlKTOvHiKqCf/3/0C5G5bbeABxNonYYQCGGxmtCD60K////wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACH5BAUUAFcALAAAAAAQABAAAAe5gFeCg4SFhkhHIyMcKoaCDExCATMIHjEoPoUMQBoiGEU3EAQJS5mDTBoWGJ8gEAkRPBQTgkgpU6s2SiBOJk8DST+CHBs2IEUCIDgmNTVRLwuCIzMlMDxQysxELVQ7ghceEg4VTcs12gY03Vcclh8uT0/mLS0GUtBXKjoyVi4N8vQnHhQYFERGBg85zhkgMaTDLEE+lszLYKDiiSEADhTyQUHBixcnQjzooNHQhB8LdnRYUOCho5eDAgEAOw==";

        private const string FalseIcon =
            "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAADSklEQVQ4jV2QS0xcZQCFv//eO3eGeTGDIHQYyqNNWtoi0sZHQjSxSdPU0ocNlVhNF7LvwsSdJhrjwoUmNLFxpYs2xhBgUQxpQkxNGjGxpQVaHg4CIhMeHcrAMHfmzn39Lui4cH3yfTnniD5VASCs+xAliy88WfeVqryTaG68Gqqp7gDIZzan0kvLNz92vcFPFLFq+3W2LZuM6yH+J3j/1ZNv3Dp04SJay0GIhAHAMHAWF5gbGuK3u/d6bb/+fVmgnlEEALqqfnb2g57rTV1dCD2AsbTA0oP7PJ15Ajs5gpEItS93kKyKXph5MhMyXW+0IOV/DU6f7Tp9J3G4FcM0+ePBOMMTj9emjcJwAZxXKkPne08cTzYkk8TqalmenaZ/5JfujOsNajtScqyl/o5P9bGYmuf+1CO+WUhf7gwFBwDuxWNM+gMnZMFIGtks29tZglqAhsb6gcziitCAt17wB5lJzWO5Dn0L6cvAwDlp81EoADBRnahuN3M5Zv0BVmybA1tbNOg6vyrikqbrWs/u6hprwJLjGcDA1eDeL8DElOa1y79SpKsquT6e/iHu2G+f0tTaZs2Hp6lXlB1FtP5tlti0LFZKpZ/bdK0MP0yrsr20u8uyX+fLp9v9wIdxV95el5JnpklAcEjLS+ycIkg5NtKVhEM6OO5DQ3gdwnYoVQS4kTf7EaLnoOuCpobSmkbItdgBTzFd71HCpxJzPOpi0XeB36Ou7MB18RSVH41SP9DT6SmojkQGK65EPY+oqpBxvUnFsN1bnqLSpgiadU2cVNXXk8B+FEaK7k9Az7V99eVZ1w5XBmm3SvhVlTlX3lSAyXHTWn8pUkEi84xwqciLiX2MOsrQ0WjkvTPhWBn+9LWW/X1tm1mO6D7GLNcBRsWbe2Frdzw8c1Hx+Md1cerqSeUM7EJxOqhoIhKLHKkK+glmNqgplRjx4POc2QmMlS+f/XY7f0mLR4bOR0IIz+R4Uz16LHpUqDq2uUthdYNiQGPYdPh61+gFxgDUxucGQzB3t2gNpG33XFUoWFkTjRGIxUHzkc/lGd/KcWN9K/udYZ7yC25bz7nyBDYEFCRIqLWg6YCgO+HzHZPAmmWn5mEwAH8WYSMiIC/3uH8BnK93p318sLgAAAAASUVORK5CYII=";

        internal bool Sortable { get; set; }

        internal bool Filterable { get; set; }

        internal bool Hidden { get; set; }

        internal Func<TModel, string> HtmlBuilder { get; private set; }

        internal Func<TModel, dynamic> ValueGetter { get; private set; }

        internal RoboUIGridColumn()
        {
            Sortable = true;
            Align = "left";
        }

        internal void SetValueGetter<TValue>(Func<TModel, TValue> expression)
        {
            ValueGetter = model => expression(model);

            HtmlBuilder = model =>
            {
                dynamic value;

                try
                {
                    value = ValueGetter(model);
                }
                catch (NullReferenceException)
                {
                    value = null;
                }

                if (value == null)
                {
                    return string.Empty;
                }

                if (value is DateTime)
                {
                    var dt = (DateTime)value;
                    if (dt == DateTime.MinValue)
                    {
                        return string.Empty;
                    }
                }

                return !string.IsNullOrEmpty(DisplayFormat)
                    ? string.Format(DisplayFormat, value)
                    : Convert.ToString(value);
            };
        }

        public RoboUIGridColumn<TModel> HasText(Func<TModel, string> expression)
        {
            SetValueGetter(expression);
            return this;
        }

        public RoboUIGridColumn<TModel> HasDisplayFormat(string value)
        {
            DisplayFormat = value;
            return this;
        }

        public RoboUIGridColumn<TModel> HasHeaderText(string value)
        {
            HeaderText = value;
            return this;
        }

        public RoboUIGridColumn<TModel> HasCssClass(string value)
        {
            CssClass = value;
            return this;
        }

        public RoboUIGridColumn<TModel> EnableFilter(bool value = true)
        {
            Filterable = value;
            return this;
        }

        public RoboUIGridColumn<TModel> HasWidth(int value)
        {
            Width = value;
            return this;
        }

        public RoboUIGridColumn<TModel> IsHidden(bool value = true)
        {
            Hidden = value;
            return this;
        }

        public RoboUIGridColumn<TModel> RenderAsHtml(Func<TModel, string> builder)
        {
            HtmlBuilder = builder;
            return this;
        }

        public RoboUIGridColumn<TModel> RenderAsLink(Func<TModel, string> href, bool targetBlank = true)
        {
            if (targetBlank)
            {
                HtmlBuilder = model => string.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", href(model));
            }
            else
            {
                HtmlBuilder = model => string.Format("<a href=\"{0}\">{0}</a>", href(model));
            }
            return this;
        }

        public RoboUIGridColumn<TModel> RenderAsLink(Func<TModel, string> text, Func<TModel, string> href, bool targetBlank = true)
        {
            if (targetBlank)
            {
                HtmlBuilder = model => string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", href(model), text(model));
            }
            else
            {
                HtmlBuilder = model => string.Format("<a href=\"{0}\">{1}</a>", href(model), text(model));
            }
            return this;
        }

        public RoboUIGridColumn<TModel> RenderAsImage(Func<TModel, string> src)
        {
            HtmlBuilder = model => string.Format("<img src=\"{0}\" />", src(model));
            return this;
        }

        public RoboUIGridColumn<TModel> RenderAsStatusImage(string trueImageSrc, string falseImageSrc)
        {
            HtmlBuilder = model =>
            {
                var value = Convert.ToBoolean(ValueGetter(model));
                return string.Format("<img src=\"{0}\" />", value ? trueImageSrc : falseImageSrc);
            };
            return this;
        }

        public RoboUIGridColumn<TModel> RenderAsStatusImage(bool reverse = false, bool showTrueOnly = false)
        {
            HtmlBuilder = model =>
            {
                var value = Convert.ToBoolean(ValueGetter(model));
                if (showTrueOnly)
                {
                    var trueImageSrc = reverse ? FalseIcon : TrueIcon;
                    return value ? string.Format("<img src=\"{0}\" />", trueImageSrc) : null;
                }
                else
                {
                    var trueImageSrc = reverse ? FalseIcon : TrueIcon;
                    var falseImageSrc = reverse ? TrueIcon : FalseIcon;
                    return string.Format("<img src=\"{0}\" />", value ? trueImageSrc : falseImageSrc);
                }
            };
            return this;
        }

        public RoboUIGridColumn<TModel> RenderAsStatusImage(Func<TModel, bool> valueGetter, bool reverse = false, bool showTrueOnly = false)
        {
            HtmlBuilder = model =>
            {
                var value = valueGetter(model);
                if (showTrueOnly)
                {
                    var trueImageSrc = reverse ? FalseIcon : TrueIcon;
                    return value ? string.Format("<img src=\"{0}\" />", trueImageSrc) : null;
                }
                else
                {
                    var trueImageSrc = reverse ? FalseIcon : TrueIcon;
                    var falseImageSrc = reverse ? TrueIcon : FalseIcon;
                    return string.Format("<img src=\"{0}\" />", value ? trueImageSrc : falseImageSrc);
                }
            };
            return this;
        }

        public RoboUIGridColumn<TModel> EnableSorting(bool value = true)
        {
            Sortable = value;
            return this;
        }

        public RoboUIGridColumn<TModel> AlignLeft()
        {
            Align = "left";
            return this;
        }

        public RoboUIGridColumn<TModel> AlignCenter()
        {
            Align = "center";
            return this;
        }

        public RoboUIGridColumn<TModel> AlignRight()
        {
            Align = "right";
            return this;
        }

        public override string BuildHtml(object item)
        {
            return HtmlBuilder((TModel)item);
        }

        public override object GetValue(object item)
        {
            return ValueGetter((TModel)item);
        }
    }
}