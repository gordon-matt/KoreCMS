using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kore.Web.Mvc.JQGrid.Enums;

namespace Kore.Web.Mvc.JQGrid
{
    public class JQColumn
    {
        #region Private Members

        private readonly List<string> classes = new List<string>();
        private readonly string columnName;
        private Align? align;
        private string customFormatter;
        private string defaultSearchValue;
        private bool? editable;
        private JQEditFormOptions editFormOptions;
        private JQEditOptions editOptions;
        private JQEditRules editRules;
        private EditType? editType;
        private bool? expandableInTree;
        private SortDirection? firstSortOrder;
        private bool? fixedWidth;
        private KeyValuePair<Formatter, string>? formatter;
        private bool? hidden;
        private string index;
        private bool? key;
        private string label;
        private bool? resizeable;
        private bool? search;
        private string searchDateFormat;
        private List<string> searchOptions = new List<string>();
        private IDictionary<string, string> searchTerms;
        private SearchType? searchType;
        private bool? sortable;
        private bool? title;
        private int? width;

        #endregion Private Members

        #region Constructor

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name = "columnName">Name of column, cannot be blank or set to 'subgrid', 'cb', and 'rn'</param>
        public JQColumn(string columnName)
        {
            // Make sure columnname is not left blank
            if (columnName.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("columnName");
            }

            // Make sure columnname is not part of the reserved names collection
            var reservedNames = new[] { "subgrid", "cb", "rn" };

            if (reservedNames.Contains(columnName))
            {
                throw new ArgumentException("Column '" + columnName + "' is reserved");
            }

            // Set columnname
            this.columnName = columnName;

            // Set index equal to columnname by default, can be overriden by setter
            index = columnName;
        }

        #endregion Constructor

        /// <summary>
        /// Identify a column if set as expandable in tree grid
        /// </summary>
        public bool IsExpandable
        {
            get { return expandableInTree ?? false; }
        }

        /// <summary>
        /// Identify if a column is set as key column
        /// </summary>
        public bool IsKey
        {
            get { return key ?? false; }
        }

        public string Name
        {
            get { return columnName; }
        }

        /// <summary>
        /// Gets default search value
        /// </summary>
        internal string DefaultSearchValue
        {
            get { return defaultSearchValue; }
        }

        /// <summary>
        /// Returns if there is an default search value set
        /// </summary>
        internal bool HasDefaultSearchValue
        {
            get { return !defaultSearchValue.IsNullOrWhiteSpace(); }
        }

        /// <summary>
        /// Gets index of column
        /// </summary>
        internal string Index
        {
            get { return index; }
        }

        internal string SearchOption
        {
            get
            {
                return searchOptions.Any()
                    ? searchOptions.First()
                    : "bw";
            }
        }

        #region Fluent Methods

        /// <summary>
        ///     This option allow to add a class to to every cell on that column. In the grid css
        ///     there is a predefined class ui-ellipsis which allow to attach ellipsis to a
        ///     particular row. Also this will work in FireFox too.
        ///     Multiple calls to this function are allowed to set multiple classes
        /// </summary>
        /// <param name = "className">Classname</param>
        public JQColumn AddClass(string className)
        {
            classes.Add(className);
            return this;
        }

        /// <summary>
        ///     Defines the alignment of the cell in the Body layer, not in header cell.
        ///     Possible values: left, center, right. (default: left)
        /// </summary>
        /// <param name = "align">Alignment of column (center, right, left</param>
        public JQColumn SetAlign(Align align)
        {
            this.align = align;
            return this;
        }

        /// <summary>
        /// Sets the column as expandable when using TreeGrid
        /// </summary>
        /// <returns></returns>
        public JQColumn SetAsExpandable()
        {
            expandableInTree = true;
            return this;
        }

        /// <summary>
        ///     Sets custom formatter. Usually this is a function. When set in the formatter option
        ///     this should not be enclosed in quotes and not entered with () -
        ///     just specify the name of the function
        ///     The following variables are passed to the function:
        ///     'cellvalue': The value to be formated (pure text).
        ///     'options': Object { rowId: rid, colModel: cm} where rowId - is the id of the row colModel is
        ///     the object of the properties for this column getted from colModel array of jqGrid
        ///     'rowobject': Row data represented in the format determined from datatype option.
        ///     If we have datatype: xml/xmlstring - the rowObject is xml node,provided according to the rules
        ///     from xmlReader If we have datatype: json/jsonstring - the rowObject is array, provided according to
        ///     the rules from jsonReader
        /// </summary>
        /// <param name = "customFormatter"></param>
        /// <returns></returns>
        public JQColumn SetCustomFormatter(string customFormatter)
        {
            if (formatter.HasValue)
            {
                throw new Exception(
                    "You cannot set a formatter and a customformatter at the same time, please choose one.");
            }
            this.customFormatter = customFormatter;
            return this;
        }

        /// <summary>
        /// Sets default search value
        /// </summary>
        /// <param name="defaultSearchValue">Default serach value</param>
        /// <returns>Column</returns>
        public JQColumn SetDefaultSearchValue(string defaultSearchValue)
        {
            this.defaultSearchValue = defaultSearchValue;
            return this;
        }

        /// <summary>
        /// Sets whether column can be edited
        /// </summary>
        /// <param name="editable"></param>
        /// <returns></returns>
        public JQColumn SetEditable(bool editable)
        {
            this.editable = editable;
            return this;
        }

        /// <summary>
        /// Sets the columns edit form options
        /// SetEditable(true) must be called for this to be respected.
        /// </summary>
        /// <param name="editFormOptions"></param>
        /// <returns></returns>
        public JQColumn SetEditFormOptions(JQEditFormOptions editFormOptions)
        {
            this.editFormOptions = editFormOptions;
            return this;
        }

        /// <summary>
        /// Sets the edit options for the column
        /// SetEditable(true) must be called for this to be respected.
        /// </summary>
        /// <param name="editOptions"></param>
        /// <returns></returns>
        public JQColumn SetEditOptions(JQEditOptions editOptions)
        {
            this.editOptions = editOptions;
            return this;
        }

        /// <summary>
        /// Sets the columns edit rules
        /// SetEditable(true) must be called for this to be respected.
        /// </summary>
        /// <param name="editRules"></param>
        /// <returns></returns>
        public JQColumn SetEditRules(JQEditRules editRules)
        {
            this.editRules = editRules;
            return this;
        }

        /// <summary>
        /// Sets the type of html element to render when column is in edit mode,
        /// SetEditable(true) must be called for this to be respected.
        /// </summary>
        /// <param name="editType"></param>
        /// <returns></returns>
        public JQColumn SetEditType(EditType editType)
        {
            this.editType = editType;
            return this;
        }

        /// <summary>
        ///     If set to asc or desc, the column will be sorted in that direction on first
        ///     sort.Subsequent sorts of the column will toggle as usual (default: null)
        /// </summary>
        /// <param name = "firstSortOrder">First sort order</param>
        public JQColumn SetFirstSortOrder(SortDirection firstSortOrder)
        {
            this.firstSortOrder = firstSortOrder;
            return this;
        }

        /// <summary>
        ///     If set to true this option does not allow recalculation of the width of the
        ///     column if shrinkToFit option is set to true. Also the width does not change
        ///     if a setGridWidth method is used to change the grid width. (default: false)
        /// </summary>
        /// <param name = "fixedWidth">Indicates if width of column is fixed</param>
        public JQColumn SetFixedWidth(bool fixedWidth)
        {
            this.fixedWidth = fixedWidth;
            return this;
        }

        /// <summary>
        ///     Sets formatter with default formatoptions (as set in language file)
        /// </summary>
        /// <param name = "formatter">Formatter</param>
        public JQColumn SetFormatter(Formatter formatter)
        {
            if (!customFormatter.IsNullOrWhiteSpace())
            {
                throw new Exception(
                    "You cannot set a formatter and a customformatter at the same time, please choose one.");
            }
            this.formatter = new KeyValuePair<Formatter, string>(formatter, "");
            return this;
        }

        /// <summary>
        ///     Sets formatter with formatoptions (see jqGrid documentation for more info on formatoptions)
        /// </summary>
        /// <param name = "formatter">Formatter</param>
        /// <param name = "formatOptions">Formatoptions</param>
        public JQColumn SetFormatter(Formatter formatter, string formatOptions)
        {
            if (!customFormatter.IsNullOrWhiteSpace())
            {
                throw new Exception(
                    "You cannot set a formatter and a customformatter at the same time, please choose one.");
            }
            this.formatter = new KeyValuePair<Formatter, string>(formatter, formatOptions);
            return this;
        }

        /// <summary>
        ///     Defines if this column is hidden at initialization. (default: false)
        /// </summary>
        /// <param name = "hidden">Boolean indicating if column is hidden</param>
        public JQColumn SetHidden(bool hidden)
        {
            this.hidden = hidden;
            return this;
        }

        /// <summary>
        ///     Set the index name when sorting. Passed as sidx parameter. (default: Same as columnname)
        /// </summary>
        /// <param name = "index">Name of index</param>
        public JQColumn SetIndex(string index)
        {
            this.index = index;
            return this;
        }

        /// <summary>
        ///     In case if there is no id from server, this can be set as as id for the unique row id.
        ///     Only one column can have this property. If there are more than one key the grid finds
        ///     the first one and the second is ignored. (default: false)
        /// </summary>
        /// <param name = "key">Indicates if key is set</param>
        public JQColumn SetKey(bool key)
        {
            this.key = key;
            return this;
        }

        /// <summary>
        ///     Defines the heading for this column. If empty, the heading for this column comes from the name property.
        /// </summary>
        /// <param name = "label">Label name of column</param>
        public JQColumn SetLabel(string label)
        {
            this.label = label;
            return this;
        }

        /// <summary>
        ///     Defines if the column can be resized (default: true)
        /// </summary>
        /// <param name = "resizeable">Indicates if the column is resizable</param>
        public JQColumn SetResizeable(bool resizeable)
        {
            this.resizeable = resizeable;
            return this;
        }

        /// <summary>
        ///     When used in search modules, disables or enables searching on that column. (default: true)
        /// </summary>
        /// <param name = "search">Indicates if searching for this column is enabled</param>
        public JQColumn SetSearch(bool search)
        {
            this.search = search;
            return this;
        }

        /// <summary>
        ///     Set dateformat of datepicker when SearchType is set to datepicker (default: dd-mm-yy)
        /// </summary>
        /// <param name = "searchDateFormat">Dateformat</param>
        public JQColumn SetSearchDateFormat(string searchDateFormat)
        {
            this.searchDateFormat = searchDateFormat;
            return this;
        }

        /// <summary>
        /// Sets search option for column
        /// </summary>
        /// <param name="searchOption">Search option</param>
        public JQColumn SetSearchOption(ConditionalOperator searchOption)
        {
            var searchOptionValues = Enum.GetValues(typeof(ConditionalOperator));
            foreach (ConditionalOperator value in searchOptionValues)
            {
                if ((searchOption & value) == value)
                {
                    switch (value)
                    {
                        case ConditionalOperator.Equal:
                            searchOptions.Add("eq");
                            break;

                        case ConditionalOperator.NotEqual:
                            searchOptions.Add("ne");
                            break;

                        case ConditionalOperator.Less:
                            searchOptions.Add("lt");
                            break;

                        case ConditionalOperator.LessOrEqual:
                            searchOptions.Add("le");
                            break;

                        case ConditionalOperator.Greater:
                            searchOptions.Add("gt");
                            break;

                        case ConditionalOperator.GreaterOrEqual:
                            searchOptions.Add("ge");
                            break;

                        case ConditionalOperator.BeginsWith:
                            searchOptions.Add("bw");
                            break;

                        case ConditionalOperator.DoesNotBeginWith:
                            searchOptions.Add("bn");
                            break;

                        case ConditionalOperator.IsIn:
                            searchOptions.Add("in");
                            break;

                        case ConditionalOperator.IsNotIn:
                            searchOptions.Add("ni");
                            break;

                        case ConditionalOperator.EndsWith:
                            searchOptions.Add("ew");
                            break;

                        case ConditionalOperator.DoesNotEndWith:
                            searchOptions.Add("en");
                            break;

                        case ConditionalOperator.Contains:
                            searchOptions.Add("cn");
                            break;

                        case ConditionalOperator.DoesNotContain:
                            searchOptions.Add("nc");
                            break;
                    }
                }
            }

            return this;
        }

        /// <summary>
        ///     Set searchterms if search type of this column is set to type select
        /// </summary>
        /// <param name = "searchTerms">Searchterm to add to dropdownlist</param>
        public JQColumn SetSearchTerms(string[] searchTerms)
        {
            this.searchTerms = searchTerms.ToDictionary(searchterm => searchterm);

            return this;
        }

        /// <summary>
        ///     Set searchterms if search type of this column is set to type select
        /// </summary>
        /// <param name = "searchTerms">Searchterm to add to dropdownlist</param>
        public JQColumn SetSearchTerms(IDictionary<string, string> searchTerms)
        {
            this.searchTerms = searchTerms;
            return this;
        }

        /// <summary>
        ///     Sets the SearchType of this column (text, select or datepicker) (default: text)
        ///     WarningSS: To use datepicker jQueryUI javascript should be included
        /// </summary>
        /// <param name = "searchType">Search type</param>
        public JQColumn SetSearchType(SearchType searchType)
        {
            this.searchType = searchType;
            return this;
        }

        /// <summary>
        ///     Indicates if column is sortable (default: true)
        /// </summary>
        /// <param name = "sortable">Indicates if column is sortable</param>
        public JQColumn SetSortable(bool sortable)
        {
            this.sortable = sortable;
            return this;
        }

        /// <summary>
        ///     If this option is false the title is not displayed in that column when we hover over a cell (default: true)
        /// </summary>
        /// <param name = "title">Indicates if title is displayed when hovering over cell</param>
        public JQColumn SetTitle(bool title)
        {
            this.title = title;
            return this;
        }

        /// <summary>
        ///     Set the initial width of the column, in pixels. This value currently can not be set as percentage (default: 150)
        /// </summary>
        /// <param name = "width">Width in pixels</param>
        public JQColumn SetWidth(int width)
        {
            this.width = width;
            return this;
        }

        #endregion Fluent Methods

        /// <summary>
        ///     Creates javascript string from column to be included in grid javascript
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var script = new StringBuilder();

            // Start column
            script.Append("{").AppendLine();

            // Align
            if (align.HasValue) script.AppendFormat("align:'{0}',", align.ToString().ToLower()).AppendLine();

            // Classes
            if (classes.Count > 0)
                script.AppendFormat("classes:'{0}',", string.Join(" ", (from c in classes select c).ToArray())).
                    AppendLine();

            // Columnname
            script.AppendFormat("name:'{0}',", columnName).AppendLine();

            // FirstSortOrder
            if (firstSortOrder.HasValue) script.AppendFormat("firstsortorder:'{0}',", firstSortOrder.ToString().ToLower()).AppendLine();

            // FixedWidth
            if (fixedWidth.HasValue)
                script.AppendFormat("fixed:{0},", fixedWidth.Value.ToString().ToLower()).AppendLine();

            // Formatters
            if (formatter.HasValue && formatter.Value.Value.IsNullOrWhiteSpace())
                script.AppendFormat("formatter:'{0}',", formatter.Value.Key.ToString().ToLower()).AppendLine();

            if (formatter.HasValue && !formatter.Value.Value.IsNullOrWhiteSpace())
                script.AppendLine("formatter:'" + formatter.Value.Key.ToString().ToLower() + "', formatoptions: {" + formatter.Value.Value + "},");

            // Custom formatter
            if (!customFormatter.IsNullOrWhiteSpace())
                script.AppendFormat("formatter:{0},", customFormatter).AppendLine();

            // Hidden
            if (hidden.HasValue) script.AppendFormat("hidden:{0},", hidden.Value.ToString().ToLower()).AppendLine();

            // Key
            if (key.HasValue) script.AppendFormat("key:{0},", key.Value.ToString().ToLower()).AppendLine();

            // Label
            if (!label.IsNullOrWhiteSpace()) script.AppendFormat("label:'{0}',", label).AppendLine();

            // Resizable
            if (resizeable.HasValue)
                script.AppendFormat("resizable:{0},", resizeable.Value.ToString().ToLower()).AppendLine();

            // Search
            if (search.HasValue && search.Value)
            {
                script.Append("search:true,").AppendLine();
            }
            else
            {
                script.Append("search:false,").AppendLine();
            }

            // SearchType
            if (searchType.HasValue)
            {
                if (searchType.Value == SearchType.Text)
                {
                    script.AppendLine("stype:'text',");
                }
                else if (searchType.Value == SearchType.Select)
                {
                    script.AppendLine("stype:'select',");
                }

                script.Append("searchoptions: {");

                if (searchOptions.Any())
                {
                    script.AppendFormat("sopt:['{0}']", searchOptions.Aggregate((current, next) => current + "',  '" + next));
                }
                else
                {
                    script.Append("sopt:['bw']");
                }
            }

            // Searchoptions
            if (searchType == SearchType.Select || searchType == SearchType.Datepicker)
            {
                // SearchType select
                if (searchType == SearchType.Select)
                {
                    if (searchTerms != null)
                    {
                        var emtpyOption = (searchTerms.Any()) ? ":;" : ":";
                        script.AppendFormat(@", value: ""{0}{1}""", emtpyOption,
                                            string.Join(";", searchTerms.Select(s => s.Key + ":" + s.Value).ToArray()));
                    }
                    else
                    {
                        script.Append(", value: ':'");
                    }
                }

                // SearchType datepicker
                if (searchType == SearchType.Datepicker)
                {
                    if (searchDateFormat.IsNullOrWhiteSpace())
                        script.Append(
                            ", dataInit:function(el){$(el).datepicker({changeYear:true, onSelect: function() {var sgrid = $('###gridid##')[0]; sgrid.triggerToolbar();},dateFormat:'dd-mm-yy'});}");
                    else
                        script.Append(
                            ", dataInit:function(el){$(el).datepicker({changeYear:true, onSelect: function() {var sgrid = $('###gridid##')[0]; sgrid.triggerToolbar();},dateFormat:'" +
                            searchDateFormat + "'});}");
                }
            }

            // SearchType
            if (searchType.HasValue)
            {
                if (!defaultSearchValue.IsNullOrWhiteSpace())
                {
                    script.AppendFormat(",defaultValue: '{0}'", defaultSearchValue);
                }

                script.AppendLine("},");
            }

            // Default value when no search type is set
            if (!searchType.HasValue && !defaultSearchValue.IsNullOrWhiteSpace())
            {
                script.Append("searchoptions: { defaultValue: '" + defaultSearchValue + "' },");
            }

            // Sortable
            if (sortable.HasValue)
                script.AppendFormat("sortable:{0},", sortable.Value.ToString().ToLower()).AppendLine();

            // Title
            if (title.HasValue) script.AppendFormat("title:{0},", title.Value.ToString().ToLower()).AppendLine();

            // Width
            if (width.HasValue) script.AppendFormat("width:{0},", width.Value).AppendLine();

            //editable
            if (editable.HasValue)
                script.AppendFormat("editable:{0},", editable.Value.ToString().ToLower()).AppendLine();

            // Searchoption
            if (searchOptions.Any() && !searchType.HasValue) // When searchtype is set, searchoptions is already added
            {
                script.AppendLine("searchoptions: { sopt:['" + searchOptions.Aggregate((current, next) => current + "', '" + next) + "'] },");
            }

            //edit type
            if (editType.HasValue)
                script.AppendFormat("edittype:'{0}',", editType.Value.ToString().ToLower()).AppendLine();

            //edit options
            if (editOptions != null)
                script.AppendFormat("editoptions:{0},", editOptions.ToString()).AppendLine();

            //edit rules
            if (editRules != null)
                script.AppendFormat("editrules:{0},", editRules.ToString()).AppendLine();

            //edit form options
            if (editFormOptions != null)
                script.AppendFormat("formoptions:{0},", editFormOptions.ToString()).AppendLine();

            // Index
            script.AppendFormat("index:'{0}'", index).AppendLine();

            // End column
            script.Append("}");

            return script.ToString();
        }
    }
}