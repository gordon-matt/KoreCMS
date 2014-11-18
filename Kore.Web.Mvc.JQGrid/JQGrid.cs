using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Kore.Collections;
using Kore.Infrastructure;
using Kore.Linq;
using Kore.Web.Mvc.JQGrid.DataReaders;
using Kore.Web.Mvc.JQGrid.Enums;
using Kore.Web.Mvc.Resources;

namespace Kore.Web.Mvc.JQGrid
{
    /// <summary>
    ///     Grid class, used to render JqGrid
    /// </summary>
    public class JQGrid<TModel> : IHtmlString where TModel : class
    {
        #region Private Members

        private string formActionUrl;

        private readonly List<JQColumn> _columns = new List<JQColumn>();
        private readonly string clientId;
        private string altClass;
        private bool? altRows;
        private bool? asyncLoad;
        private bool? autoEncode;
        private bool? autoWidth;
        private string caption;
        private DataType dataType = DataType.Json;
        private string emptyRecords;
        private bool enabledTreeGrid;
        private bool? footerRow;
        private bool? forceFit;
        private bool? gridView;
        private bool? headerTitles;
        private int? height;
        private bool? hiddenGrid;
        private bool? hideGrid;
        private bool? hoverRows;
        private bool? ignoreCase;
        private JsonReader jsonReader;
        private bool? loadOnce;
        private string loadText;
        private LoadUIOption? loadUI;
        private bool? multiBoxOnly;
        private MultiKey? multiKey;
        private bool? multiSelect;
        private int? multiSelectWidth;
        private string onAfterInsertRow;
        private string onBeforeRequest;
        private string onBeforeSelectRow;
        private string onCellSelect;
        private string onDblClickRow;
        private string onGridComplete;
        private string onHeaderClick;
        private string onLoadBeforeSend;
        private string onLoadComplete;
        private string onLoadError;
        private string onPaging;
        private string onResizeStart;
        private string onResizeStop;
        private string onRightClickRow;
        private string onSelectAll;
        private string onSelectRow;
        private string onSerializeGridData;
        private string onSortCol;
        private int? page;
        private string pager;
        private PagerPosition? pagerPos;
        private bool? pgButtons;
        private bool? pgInput;
        private string pgText;
        private RecordPosition? recordPos;
        private string recordText;
        private FormMethod? requestType;
        private string resizeClass;
        private int[] rowList;
        private int? rowNum;
        private bool? rowNumbers;
        private int? rowNumWidth;
        private bool? scroll;
        private int? scrollInt;
        private int? scrollOffset;
        private bool? scrollRows;
        private int? scrollTimeout;
        private bool? searchClearButton;
        private bool? searchOnEnter;
        private bool? searchToggleButton;
        private bool? searchToolbar;
        private bool? showAllSortIcons;
        private bool? shrinkToFit;
        private Direction? sortIconDirection;
        private string sortName;
        private bool? sortOnHeaderClick;
        private SortDirection? sortOrder;
        private bool stringResult = true;
        private bool? toolbar;
        private ToolbarPosition toolbarPosition = ToolbarPosition.Top;
        private bool? topPager;
        private TreeGridModel treeGridModel;
        private int? treeGridRootLevel;
        private string url;
        private bool? userDataOnFooter;
        private bool? viewRecords;
        private int? width;

        private Dictionary<string, string> postData = new Dictionary<string, string>();

        private bool responsive = true;

        #endregion Private Members

        #region Constructor

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name = "id">Id of grid</param>
        public JQGrid(string id)
        {
            if (id.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("Id must contain a value to identify the grid");
            }
            clientId = id;
        }

        #endregion Constructor

        #region Fluid Methods

        /// <summary>
        ///     Adds columns to grid
        /// </summary>
        /// <param name = "column">Colomn object</param>
        public JQGrid<TModel> AddColumn(JQColumn column)
        {
            _columns.Add(column);
            return this;
        }

        //TODO: Add JQGridAttribute to view model properties to better control this
        public JQGrid<TModel> AddColumnFor<TValue>(Expression<Func<TModel, TValue>> expression, string headerText = null)
        {
            var column = new JQColumn(Utils.GetFullPropertyName(expression));
            if (headerText != null)
            {
                column = column.SetLabel(headerText);
            }
            _columns.Add(column);
            return this;
        }

        public JQGrid<TModel> AddActionsColumn(string headerText = null, int width = 150, bool isFixedWith = true)
        {
            var column = new JQColumn("_RowActions");

            if (headerText != null)
            {
                column = column.SetLabel(headerText);
            }
            else
            {
                column = column.SetLabel("Actions");
            }

            if (width > 0)
            {
                column = column.SetWidth(width);
            }

            column = column.SetFixedWidth(isFixedWith);

            _columns.Add(column);
            return this;
        }

        /// <summary>
        ///     Adds a number of columns to grid
        /// </summary>
        /// <param name = "columns">IEnumerable of Colomn objects to add to the grid</param>
        public JQGrid<TModel> AddColumns(IEnumerable<JQColumn> columns)
        {
            _columns.AddRange(columns);
            return this;
        }

        public JQGrid<TModel> EnableTreeGrid(TreeGridModel treeGridModel = TreeGridModel.Adjacency, int rootLevel = 0)
        {
            treeGridRootLevel = 0;
            enabledTreeGrid = true;
            this.treeGridModel = treeGridModel;

            return this;
        }

        /// <summary>
        ///     This event fires after each inserted row.
        ///     Variables available in call:
        ///     'rowid': Id of the inserted row
        ///     'rowdata': An array of the data to be inserted into the row. This is array of type name-
        ///     value, where the name is a name from colModel
        ///     'rowelem': The element from the response. If the data is xml this is the xml element of the row;
        ///     if the data is json this is array containing all the data for the row
        ///     Warning: this event does not fire if gridview option is set to true
        /// </summary>
        /// <param name = "onAfterInsertRow">Script to be executed</param>
        public JQGrid<TModel> OnAfterInsertRow(string onAfterInsertRow)
        {
            this.onAfterInsertRow = onAfterInsertRow;
            return this;
        }

        /// <summary>
        ///     This event fires before requesting any data. Does not fire if datatype is function
        ///     Variables available in call: None
        /// </summary>
        /// <param name = "onBeforeRequest">Script to be executed</param>
        public JQGrid<TModel> OnBeforeRequest(string onBeforeRequest)
        {
            this.onBeforeRequest = onBeforeRequest;
            return this;
        }

        /// <summary>
        ///     This event fires when the user clicks on the row, but before selecting it.
        ///     Variables available in call:
        ///     'rowid': The id of the row.
        ///     'e': The event object
        ///     This event should return boolean true or false. If the event returns true the selection
        ///     is done. If the event returns false the row is not selected and any other action if defined
        ///     does not occur.
        /// </summary>
        /// <param name = "onBeforeSelectRow">Script to be executed</param>
        public JQGrid<TModel> OnBeforeSelectRow(string onBeforeSelectRow)
        {
            this.onBeforeSelectRow = onBeforeSelectRow;
            return this;
        }

        /// <summary>
        ///     Fires when we click on a particular cell in the grid.
        ///     Variables available in call:
        ///     'rowid': The id of the row
        ///     'iCol': The index of the cell,
        ///     'cellcontent': The content of the cell,
        ///     'e': The event object element where we click.
        ///     (Be aware that this available when we are not using cell editing module
        ///     and is disabled when using cell editing).
        /// </summary>
        /// <param name = "onCellSelect">Script to be executed</param>
        public JQGrid<TModel> OnCellSelect(string onCellSelect)
        {
            this.onCellSelect = onCellSelect;
            return this;
        }

        /// <summary>
        ///     Raised immediately after row was double clicked.
        ///     Variables available in call:
        ///     'rowid': The id of the row,
        ///     'iRow': The index of the row (do not mix this with the rowid),
        ///     'iCol': The index of the cell.
        ///     'e': The event object
        /// </summary>
        /// <param name = "onDblClickRow">Script to be executed</param>
        public JQGrid<TModel> OnDblClickRow(string onDblClickRow)
        {
            this.onDblClickRow = onDblClickRow;
            return this;
        }

        /// <summary>
        ///     This fires after all the data is loaded into the grid and all the other processes are complete.
        ///     Also the event fires independent from the datatype parameter and after sorting paging and etc.
        ///     Variables available in call: None
        /// </summary>
        /// <param name = "onGridComplete">Script to be executed</param>
        public JQGrid<TModel> OnGridComplete(string onGridComplete)
        {
            this.onGridComplete = onGridComplete;
            return this;
        }

        /// <summary>
        ///     Fires after clicking hide or show grid (hidegrid:true)
        ///     Variables available in call:
        ///     'gridstate': The state of the grid - can have two values - visible or hidden
        /// </summary>
        /// <param name = "onHeaderClick">Script to be executed</param>
        public JQGrid<TModel> OnHeaderClick(string onHeaderClick)
        {
            this.onHeaderClick = onHeaderClick;
            return this;
        }

        /// <summary>
        ///     A pre-callback to modify the XMLHttpRequest object (xhr) before it is sent. Use this to set
        ///     custom headers etc. The XMLHttpRequest is passed as the only argument.
        ///     Variables available in call:
        ///     'xhr': The XMLHttpRequest
        /// </summary>
        /// <param name = "onLoadBeforeSend">Script to be executed</param>
        public JQGrid<TModel> OnLoadBeforeSend(string onLoadBeforeSend)
        {
            this.onLoadBeforeSend = onLoadBeforeSend;
            return this;
        }

        /// <summary>
        ///     This event is executed immediately after every server request.
        ///     Variables available in call:
        ///     'xhr': The XMLHttpRequest
        /// </summary>
        /// <param name = "onLoadComplete">Script to be executed</param>
        public JQGrid<TModel> OnLoadComplete(string onLoadComplete)
        {
            this.onLoadComplete = onLoadComplete;
            return this;
        }

        /// <summary>
        ///     A function to be called if the request fails.
        ///     Variables available in call:
        ///     'xhr': The XMLHttpRequest
        ///     'status': String describing the type of error
        ///     'error': Optional exception object, if one occurred
        /// </summary>
        /// <param name = "onLoadError">Script to be executed</param>
        public JQGrid<TModel> OnLoadError(string onLoadError)
        {
            this.onLoadError = onLoadError;
            return this;
        }

        /// <summary>
        ///     This event fires after click on [page button] and before populating the data.
        ///     Also works when the user enters a new page number in the page input box
        ///     (and presses [Enter]) and when the number of requested records is changed via
        ///     the select box.
        ///     If this event returns 'stop' the processing is stopped and you can define your
        ///     own custom paging
        ///     Variables available in call:
        ///     'pgButton': first,last,prev,next in case of button click, records in case when
        ///     a number of requested rows is changed and user when the user change the number of the requested page
        /// </summary>
        /// <param name = "onPaging">Script to be executed</param>
        public JQGrid<TModel> OnPaging(string onPaging)
        {
            this.onPaging = onPaging;
            return this;
        }

        /// <summary>
        ///     Event which is called when we start resizing a column.
        ///     Variables available in call:
        ///     'event':  The event object
        ///     'index': The index of the column in colModel.
        /// </summary>
        /// <param name = "onResizeStart">Script to be executed</param>
        public JQGrid<TModel> OnResizeStart(string onResizeStart)
        {
            this.onResizeStart = onResizeStart;
            return this;
        }

        /// <summary>
        ///     Event which is called after the column is resized.
        ///     Variables available in call:
        ///     'newwidth': The new width of the column
        ///     'index': The index of the column in colModel.
        /// </summary>
        /// <param name = "onResizeStop">Script to be executed</param>
        public JQGrid<TModel> OnResizeStop(string onResizeStop)
        {
            this.onResizeStop = onResizeStop;
            return this;
        }

        /// <summary>
        ///     Raised immediately after row was right clicked.
        ///     Variables available in call:
        ///     'rowid': The id of the row,
        ///     'iRow': The index of the row (do not mix this with the rowid),
        ///     'iCol': The index of the cell.
        ///     'e': The event object
        ///     Warning - this event does not work in Opera browsers, since Opera does not support oncontextmenu event
        /// </summary>
        /// <param name = "onRightClickRow">Script to be executed</param>
        public JQGrid<TModel> OnRightClickRow(string onRightClickRow)
        {
            this.onRightClickRow = onRightClickRow;
            return this;
        }

        /// <summary>
        ///     This event fires when multiselect option is true and you click on the header checkbox.
        ///     Variables available in call:
        ///     'aRowids':  Array of the selected rows (rowid's).
        ///     'status': Boolean variable determining the status of the header check box - true if checked, false if not checked.
        ///     Be awareS that the aRowids alway contain the ids when header checkbox is checked or unchecked.
        /// </summary>
        /// <param name = "onSelectAll">Script to be executed</param>
        public JQGrid<TModel> OnSelectAll(string onSelectAll)
        {
            this.onSelectAll = onSelectAll;
            return this;
        }

        /// <summary>
        ///     Raised immediately when row is clicked.
        ///     Variables available in function call:
        ///     'rowid': The id of the row,
        ///     'status': Tthe status of the selection. Can be used when multiselect is set to true.
        ///     true if the row is selected, false if the row is deselected.
        ///     <param name = "onSelectRow">Script to be executed</param>
        /// </summary>
        public JQGrid<TModel> OnSelectRow(string onSelectRow)
        {
            this.onSelectRow = onSelectRow;
            return this;
        }

        /// <summary>
        ///     If this event is set it can serialize the data passed to the ajax request.
        ///     The function should return the serialized data. This event can be used when
        ///     custom data should be passed to the server - e.g - JSON string, XML string and etc.
        ///     Variables available in call:
        ///     'postData': Posted data
        /// </summary>
        /// <param name = "onSerializeGridData">Script to be executed</param>
        public JQGrid<TModel> OnSerializeGridData(string onSerializeGridData)
        {
            this.onSerializeGridData = onSerializeGridData;
            return this;
        }

        /// <summary>
        ///     Raised immediately after sortable column was clicked and before sorting the data.
        ///     Variables available in call:
        ///     'index': The index name from colModel
        ///     'iCol': The index of column,
        ///     'sortorder': The new sorting order - can be 'asc' or 'desc'.
        ///     If this event returns 'stop' the sort processing is stopped and you can define your own custom sorting
        /// </summary>
        /// <param name = "onSortCol">Script to be executed</param>
        public JQGrid<TModel> OnSortCol(string onSortCol)
        {
            this.onSortCol = onSortCol;
            return this;
        }

        /// <summary>
        ///     The class that is used for alternate rows. You can construct your own class and replace this value.
        ///     This option is valid only if altRows options is set to true (default: ui-priority-secondary)
        /// </summary>
        /// <param name = "altClass">Classname for alternate rows</param>
        public JQGrid<TModel> SetAltClass(string altClass)
        {
            this.altClass = altClass;
            return this;
        }

        /// <summary>
        ///     Set a zebra-striped grid (default: false)
        /// </summary>
        /// <param name = "altRows">Boolean indicating if zebra-striped grid is used</param>
        public JQGrid<TModel> SetAltRows(Boolean altRows)
        {
            this.altRows = altRows;
            return this;
        }

        /// <summary>
        /// Set to true when page is being added to document asyncronously,
        /// prevents javascript from being wrapped in $(document).ready()
        /// </summary>
        /// <param name="asyncPageLoad"></param>
        /// <returns></returns>
        public JQGrid<TModel> SetAsyncLoad(bool asyncPageLoad)
        {
            asyncLoad = asyncPageLoad;
            return this;
        }

        /// <summary>
        ///     When set to true encodes (html encode) the incoming (from server) and posted
        ///     data (from editing modules). (default: false)
        /// </summary>
        /// <param name = "autoEncode">Boolean indicating if autoencode is used</param>
        public JQGrid<TModel> SetAutoEncode(bool autoEncode)
        {
            this.autoEncode = autoEncode;
            return this;
        }

        /// <summary>
        ///     When set to true, the grid width is recalculated automatically to the width of the
        ///     parent element. This is done only initially when the grid is created. In order to
        ///     resize the grid when the parent element changes width you should apply custom code
        ///     and use a setGridWidth method for this purpose. (default: false)
        /// </summary>
        /// <param name = "autoWidth">Boolean indicating if autowidth is used</param>
        public JQGrid<TModel> SetAutoWidth(bool autoWidth)
        {
            this.autoWidth = autoWidth;
            return this;
        }

        /// <summary>
        ///     Defines the caption layer for the grid. This caption appears above the header layer.
        ///     If the string is empty the caption does not appear. (default: empty)
        /// </summary>
        /// <param name = "caption">Caption of grid</param>
        public JQGrid<TModel> SetCaption(string caption)
        {
            this.caption = caption;
            return this;
        }

        /// <summary>
        ///     Defines what type of information to expect to represent data in the grid. Valid
        ///     options are json (default) and xml
        /// </summary>
        /// <param name = "dataType">Data type</param>
        public JQGrid<TModel> SetDataType(DataType dataType)
        {
            this.dataType = dataType;
            return this;
        }

        /// <summary>
        ///     Displayed when the returned (or the current) number of records is zero.
        ///     This option is valid only if viewrecords option is set to true. (default value is
        ///     set in language file)
        /// </summary>
        /// <param name = "emptyRecords">Display string</param>
        public JQGrid<TModel> SetEmptyRecords(string emptyRecords)
        {
            this.emptyRecords = emptyRecords;
            return this;
        }

        /// <summary>
        ///     If set to true this will place a footer table with one row below the grid records
        ///     and above the pager. The number of columns equal to the number of columns in the colModel
        ///     (default: false)
        /// </summary>
        /// <param name = "footerRow">Boolean indicating whether footerrow is displayed</param>
        public JQGrid<TModel> SetFooterRow(bool footerRow)
        {
            this.footerRow = footerRow;
            return this;
        }

        /// <summary>
        ///     If set to true, when resizing the width of a column, the adjacent column (to the right)
        ///     will resize so that the overall grid width is maintained (e.g., reducing the width of
        ///     column 2 by 30px will increase the size of column 3 by 30px).
        ///     In this case there is no horizontal scrolbar.
        ///     Warning: this option is not compatible with shrinkToFit option - i.e if
        ///     shrinkToFit is set to false, forceFit is ignored.
        /// </summary>
        /// <param name = "forceFit">Boolean indicating if forcefit is enforced</param>
        public JQGrid<TModel> SetForceFit(bool forceFit)
        {
            this.forceFit = forceFit;
            return this;
        }

        /// <summary>
        ///     In the previous versions of jqGrid including 3.4.X,reading relatively big data sets
        ///     (Rows >=100 ) caused speed problems. The reason for this was that as every cell was
        ///     inserted into the grid we applied about 5-6 jQuery calls to it. Now this problem has
        ///     been resolved; we now insert the entry row at once with a jQuery append. The result is
        ///     impressive - about 3-5 times faster. What will be the result if we insert all the
        ///     data at once? Yes, this can be done with help of the gridview option. When set to true,
        ///     the result is a grid that is 5 to 10 times faster. Of course when this option is set
        ///     to true we have some limitations. If set to true we can not use treeGrid, subGrid,
        ///     or afterInsertRow event. If you do not use these three options in the grid you can
        ///     set this option to true and enjoy the speed. (default: false)
        /// </summary>
        /// <param name = "gridView">Boolean indicating gridview is enabled</param>
        public JQGrid<TModel> SetGridView(bool gridView)
        {
            this.gridView = gridView;
            return this;
        }

        /// <summary>
        ///     If the option is set to true the title attribute is added to the column headers (default: false)
        /// </summary>
        /// <param name = "headerTitles">Boolean indicating if headertitles are enabled</param>
        public JQGrid<TModel> SetHeaderTitles(bool headerTitles)
        {
            this.headerTitles = headerTitles;
            return this;
        }

        /// <summary>
        ///     The height of the grid in pixels (default: 100%, which is the only acceptable percentage for jqGrid)
        /// </summary>
        /// <param name = "height">Height in pixels</param>
        public JQGrid<TModel> SetHeight(int height)
        {
            this.height = height;
            return this;
        }

        /// <summary>
        ///     If set to true the grid initially is hidden. The data is not loaded (no request is sent) and only the
        ///     caption layer is shown. When the show/hide button is clicked for the first time to show the grid, the request
        ///     is sent to the server, the data is loaded, and the grid is shown. From this point on we have a regular grid.
        ///     This option has effect only if the caption property is not empty. (default: false)
        /// </summary>
        /// <param name = "hiddenGrid">Boolean indicating if hiddengrid is enforced</param>
        public JQGrid<TModel> SetHiddenGrid(bool hiddenGrid)
        {
            this.hiddenGrid = hiddenGrid;
            return this;
        }

        /// <summary>
        ///     Enables or disables the show/hide grid button, which appears on the right side of the caption layer.
        ///     Takes effect only if the caption property is not an empty string. (default: true)
        /// </summary>
        /// <param name = "hideGrid">Boolean indicating if show/hide button is enabled</param>
        public JQGrid<TModel> SetHideGrid(bool hideGrid)
        {
            this.hideGrid = hideGrid;
            return this;
        }

        /// <summary>
        ///     When set to false the mouse hovering is disabled in the grid data rows. (default: true)
        /// </summary>
        /// <param name = "hoverRows">Indicates whether hoverrows is enabled</param>
        public JQGrid<TModel> SetHoverRows(bool hoverRows)
        {
            this.hoverRows = hoverRows;
            return this;
        }

        /// <summary>
        /// Set to true when filtering grid loaded with SetLoadOnce(true)
        /// to filter the data case insesitive
        /// </summary>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public JQGrid<TModel> SetIgnoreCase(bool ignoreCase)
        {
            this.ignoreCase = ignoreCase;
            return this;
        }

        /// <summary>
        /// JSON data is handled in a fashion very similar to that of xml data. What is important is that the definition of the jsonReader matches the data being received
        /// </summary>
        /// <param name="jsonReader"></param>
        /// <returns></returns>
        public JQGrid<TModel> SetJsonReader(JsonReader jsonReader)
        {
            this.jsonReader = jsonReader;
            return this;
        }

        /// <summary>
        ///     If this flag is set to true, the grid loads the data from the server only once (using the
        ///     appropriate datatype). After the first request the datatype parameter is automatically
        ///     changed to local and all further manipulations are done on the client side. The functions
        ///     of the pager (if present) are disabled. (default: false)
        /// </summary>
        /// <param name = "loadOnce">Boolean indicating if loadonce is enforced</param>
        public JQGrid<TModel> SetLoadOnce(bool loadOnce)
        {
            this.loadOnce = loadOnce;
            return this;
        }

        /// <summary>
        ///     The text which appears when requesting and sorting data. This parameter override the value located
        ///     in the language file
        /// </summary>
        /// <param name = "loadText">Loadtext</param>
        public JQGrid<TModel> SetLoadText(string loadText)
        {
            this.loadText = loadText;
            return this;
        }

        /// <summary>
        ///     This option controls what to do when an ajax operation is in progress.
        ///     'disable' - disables the jqGrid progress indicator. This way you can use your own indicator.
        ///     'enable' (default) - enables “Loading” message in the center of the grid.
        ///     'block' - enables the “Loading” message and blocks all actions in the grid until the ajax request
        ///     is finished. Be aware that this disables paging, sorting and all actions on toolbar, if any.
        /// </summary>
        /// <param name = "loadUI">Load ui mode</param>
        public JQGrid<TModel> SetLoadUi(LoadUIOption loadUI)
        {
            this.loadUI = loadUI;
            return this;
        }

        /// <summary>
        ///     This option works only when multiselect = true. When multiselect is set to true, clicking anywhere
        ///     on a row selects that row; when multiboxonly is also set to true, the multiselection is done only
        ///     when the checkbox is clicked (Yahoo style). Clicking in any other row (suppose the checkbox is
        ///     not clicked) deselects all rows and the current row is selected. (default: false)
        /// </summary>
        /// <param name = "multiBoxOnly">Boolean indicating if multiboxonly is enforced</param>
        public JQGrid<TModel> SetMultiBoxOnly(bool multiBoxOnly)
        {
            this.multiBoxOnly = multiBoxOnly;
            return this;
        }

        /// <summary>
        ///     This parameter makes sense only when multiselect option is set to true.
        ///     Defines the key which will be pressed
        ///     when we make a multiselection. The possible values are:
        ///     'shiftKey' - the user should press Shift Key
        ///     'altKey' - the user should press Alt Key
        ///     'ctrlKey' - the user should press Ctrl Key
        /// </summary>
        /// <param name = "multiKey">Key to multiselect</param>
        public JQGrid<TModel> SetMultiKey(MultiKey multiKey)
        {
            this.multiKey = multiKey;
            return this;
        }

        /// <summary>
        ///     If this flag is set to true a multi selection of rows is enabled. A new column
        ///     at the left side is added. Can be used with any datatype option. (default: false)
        /// </summary>
        /// <param name = "multiSelect">Boolean indicating if multiselect is enabled</param>
        public JQGrid<TModel> SetMultiSelect(bool multiSelect)
        {
            this.multiSelect = multiSelect;
            return this;
        }

        /// <summary>
        ///     Determines the width of the multiselect column if multiselect is set to true. (default: 20)
        /// </summary>
        /// <param name = "multiSelectWidth"></param>
        public JQGrid<TModel> SetMultiSelectWidth(int multiSelectWidth)
        {
            this.multiSelectWidth = multiSelectWidth;
            return this;
        }

        /// <summary>
        ///     Set the initial number of selected page when we make the request.This parameter is passed to the url
        ///     for use by the server routine retrieving the data (default: 1)
        /// </summary>
        /// <param name = "page">Number of page</param>
        public JQGrid<TModel> SetPage(int page)
        {
            this.page = page;
            return this;
        }

        /// <summary>
        ///     If pagername is specified a pagerelement is automatically added to the grid
        /// </summary>
        /// <param name = "pager">Id/name of pager</param>
        public JQGrid<TModel> SetPager(string pager)
        {
            this.pager = pager;
            return this;
        }

        /// <summary>
        ///     Determines the position of the pager in the grid. By default the pager element
        ///     when created is divided in 3 parts (one part for pager, one part for navigator
        ///     buttons and one part for record information) (default: center)
        /// </summary>
        /// <param name = "pagerPos">Position of pager</param>
        public JQGrid<TModel> SetPagerPos(PagerPosition pagerPos)
        {
            this.pagerPos = pagerPos;
            return this;
        }

        /// <summary>
        ///     Determines if the pager buttons should be displayed if pager is available. Valid
        ///     only if pager is set correctly. The buttons are placed in the pager bar. (default: true)
        /// </summary>
        /// <param name = "pgButtons">Boolean indicating if pager buttons are displayed</param>
        public JQGrid<TModel> SetPgButtons(bool pgButtons)
        {
            this.pgButtons = pgButtons;
            return this;
        }

        /// <summary>
        ///     Determines if the input box, where the user can change the number of the requested page,
        ///     should be available. The input box appears in the pager bar. (default: true)
        /// </summary>
        /// <param name = "pgInput">Boolean indicating if pager input is available</param>
        public JQGrid<TModel> SetPgInput(bool pgInput)
        {
            this.pgInput = pgInput;
            return this;
        }

        /// <summary>
        ///     Show information about current page status. The first value is the current loaded page.
        ///     The second value is the total number of pages (default is set in language file)
        ///     Example: "Page {0} of {1}"
        /// </summary>
        /// <param name = "pgText">Current page status text</param>
        public JQGrid<TModel> SetPgText(string pgText)
        {
            this.pgText = pgText;
            return this;
        }

        /// <summary>
        ///     Determines the position of the record information in the pager. Can be left, center, right
        ///     (default: right)
        ///     Warning: When pagerpos en recordpos are equally set, pager is hidden.
        /// </summary>
        /// <param name = "recordPos">Position of record information</param>
        public JQGrid<TModel> SetRecordPos(RecordPosition recordPos)
        {
            this.recordPos = recordPos;
            return this;
        }

        /// <summary>
        ///     Represent information that can be shown in the pager. This option is valid if viewrecords
        ///     option is set to true. This text appears only if the total number of records is greater then
        ///     zero.
        ///     In order to show or hide information the items between {} mean the following: {0} the
        ///     start position of the records depending on page number and number of requested records;
        ///     {1} - the end position {2} - total records returned from the data (default defined in language file)
        /// </summary>
        /// <param name = "recordText">Record Text</param>
        public JQGrid<TModel> SetRecordText(string recordText)
        {
            this.recordText = recordText;
            return this;
        }

        /// <summary>
        ///     Defines the type of request to make (“POST” or “GET”) (default: GET)
        /// </summary>
        /// <param name = "requestType">Request type</param>
        public JQGrid<TModel> SetRequestType(FormMethod requestType)
        {
            this.requestType = requestType;
            return this;
        }

        /// <summary>
        /// Additional data to send to the server.
        /// </summary>
        /// <param name="idSelector">The ID of the HTML tag whose value to add to "postData".</param>
        /// <returns></returns>
        public JQGrid<TModel> AddPostData(string idSelector)
        {
            AddPostData(idSelector, idSelector);
            return this;
        }

        /// <summary>
        /// Additional data to send to the server.
        /// </summary>
        /// <param name="name">The name of the value to add to "postData"</param>
        /// <param name="idSelector">The ID of the HTML tag whose value to add to "postData".</param>
        /// <returns></returns>
        public JQGrid<TModel> AddPostData(string name, string idSelector)
        {
            postData.Add(name, "function(){ return $('#" + idSelector + "').val(); }");
            return this;
        }

        /// <summary>
        ///     Assigns a class to columns that are resizable so that we can show a resize
        ///     handle (default: empty string)
        /// </summary>
        /// <param name = "resizeClass"></param>
        /// <returns></returns>
        public JQGrid<TModel> SetResizeClass(string resizeClass)
        {
            this.resizeClass = resizeClass;
            return this;
        }

        /// <summary>
        ///     An array to construct a select box element in the pager in which we can change the number
        ///     of the visible rows. When changed during the execution, this parameter replaces the rowNum
        ///     parameter that is passed to the url. If the array is empty the element does not appear
        ///     in the pager. Typical you can set this like [10,20,30]. If the rowNum parameter is set to
        ///     30 then the selected value in the select box is 30.
        /// </summary>
        /// <example>
        ///     setRowList(new int[]{10,20,50})
        /// </example>
        /// <param name = "rowList">List of rows per page</param>
        public JQGrid<TModel> SetRowList(int[] rowList)
        {
            this.rowList = rowList;
            return this;
        }

        /// <summary>
        ///     Sets how many records we want to view in the grid. This parameter is passed to the url
        ///     for use by the server routine retrieving the data. Be aware that if you set this parameter
        ///     to 10 (i.e. retrieve 10 records) and your server return 15 then only 10 records will be
        ///     loaded. Set this parameter to -1 (unlimited) to disable this checking. (default: 20)
        /// </summary>
        /// <param name = "rowNum">Number of rows per page</param>
        public JQGrid<TModel> SetRowNum(int rowNum)
        {
            this.rowNum = rowNum;
            return this;
        }

        /// <summary>
        ///     If this option is set to true, a new column at the leftside of the grid is added. The purpose of
        ///     this column is to count the number of available rows, beginning from 1. In this case
        ///     colModel is extended automatically with a new element with name - 'rn'. Also, be careful
        ///     not to use the name 'rn' in colModel
        /// </summary>
        /// <param name = "rowNumbers">Boolean indicating if rownumbers are enabled</param>
        public JQGrid<TModel> SetRowNumbers(bool rowNumbers)
        {
            this.rowNumbers = rowNumbers;
            return this;
        }

        /// <summary>
        ///     Determines the width of the row number column if rownumbers option is set to true. (default: 25)
        /// </summary>
        /// <param name = "rowNumWidth">Width of rownumbers column</param>
        public JQGrid<TModel> SetRowNumWidth(int rowNumWidth)
        {
            this.rowNumWidth = rowNumWidth;
            return this;
        }

        /// <summary>
        ///     Creates dynamic scrolling grids. When enabled, the pager elements are disabled and we can use the
        ///     vertical scrollbar to load data. When set to true the grid will always hold all the items from the
        ///     start through to the latest point ever visited.
        ///     When scroll is set to an integer value (eg 1), the grid will just hold the visible lines. This allow us to
        ///     load the data at portions whitout to care about the memory leaks. (default: false)
        /// </summary>
        /// <param name = "scroll">Boolean indicating if scroll is enforced</param>
        [Obsolete("Method is obsolete, use SetVirtualScroll instead")]
        public JQGrid<TModel> SetScroll(bool scroll)
        {
            this.scroll = scroll;
            return this;
        }

        /// <summary>
        ///     Creates dynamic scrolling grids. When enabled, the pager elements are disabled and we can use the
        ///     vertical scrollbar to load data. When set to true the grid will always hold all the items from the
        ///     start through to the latest point ever visited.
        ///     When scroll is set to an integer value (eg 1), the grid will just hold the visible lines. This allow us to
        ///     load the data at portions whitout to care about the memory leaks. (default: false)
        /// </summary>
        /// <param name = "scroll">When integer value is set (eg 1) scroll is enforced</param>
        [Obsolete("Method is obsolete, use SetVirtualScroll instead")]
        public JQGrid<TModel> SetScroll(int scroll)
        {
            scrollInt = scroll;
            return this;
        }

        /// <summary>
        ///     Determines the width of the vertical scrollbar. Since different browsers interpret this width
        ///     differently (and it is difficult to calculate it in all browsers) this can be changed. (default: 18)
        /// </summary>
        /// <param name = "scrollOffset">Scroll offset</param>
        public JQGrid<TModel> SetScrollOffset(int scrollOffset)
        {
            this.scrollOffset = scrollOffset;
            return this;
        }

        /// <summary>
        ///     When enabled, selecting a row with setSelection scrolls the grid so that the selected row is visible.
        ///     This is especially useful when we have a verticall scrolling grid and we use form editing with
        ///     navigation buttons (next or previous row). On navigating to a hidden row, the grid scrolls so the
        ///     selected row becomes visible. (default: false)
        /// </summary>
        /// <param name = "scrollRows">Boolean indicating if scrollrows is enabled</param>
        public JQGrid<TModel> SetScrollRows(bool scrollRows)
        {
            this.scrollRows = scrollRows;
            return this;
        }

        /// <summary>
        ///     This controls the timeout handler when scroll is set to 1. (default: 200 milliseconds)
        /// </summary>
        /// <param name = "scrollTimeout">Scroll timeout in milliseconds</param>
        /// <returns></returns>
        public JQGrid<TModel> SetScrollTimeout(int scrollTimeout)
        {
            this.scrollTimeout = scrollTimeout;
            return this;
        }

        /// <summary>
        ///     When set to true adds clear button to clear all search entries (default: false)
        /// </summary>
        /// <param name = "searchClearButton"></param>
        /// <returns></returns>
        public JQGrid<TModel> SetSearchClearButton(bool searchClearButton)
        {
            this.searchClearButton = searchClearButton;
            return this;
        }

        /// <summary>
        ///     Determines how the search should be applied. If this option is set to true search is started when
        ///     the user hits the enter key. If the option is false then the search is performed immediately after
        ///     the user presses some character. (default: true
        /// </summary>
        /// <param name = "searchOnEnter">Indicates if search is started on enter</param>
        public JQGrid<TModel> SetSearchOnEnter(bool searchOnEnter)
        {
            this.searchOnEnter = searchOnEnter;
            return this;
        }

        /// <summary>
        ///     When set to true adds toggle button to toggle search toolbar (default: false)
        /// </summary>
        /// <param name = "searchToggleButton">Indicates if toggle button is displayed</param>
        public JQGrid<TModel> SetSearchToggleButton(bool searchToggleButton)
        {
            this.searchToggleButton = searchToggleButton;
            return this;
        }

        /// <summary>
        ///     Enables toolbar searching / filtering
        /// </summary>
        /// <param name = "searchToolbar">Indicates if toolbar searching is enabled</param>
        public JQGrid<TModel> SetSearchToolbar(bool searchToolbar)
        {
            this.searchToolbar = searchToolbar;
            return this;
        }

        /// <summary>
        ///     If enabled all sort icons are visible for all columns which are sortable (default: false)
        /// </summary>
        /// <param name = "showAllSortIcons">Boolean indicating if all sorting icons should be displayed</param>
        public JQGrid<TModel> SetShowAllSortIcons(bool showAllSortIcons)
        {
            this.showAllSortIcons = showAllSortIcons;
            return this;
        }

        /// <summary>
        ///     This option describes the type of calculation of the initial width of each column
        ///     against the width of the grid. If the value is true and the value in width option
        ///     is set then: Every column width is scaled according to the defined option width.
        ///     Example: if we define two columns with a width of 80 and 120 pixels, but want the
        ///     grid to have a 300 pixels - then the columns are recalculated as follow:
        ///     1- column = 300(new width)/200(sum of all width)*80(column width) = 120 and 2 column = 300/200*120 = 180.
        ///     The grid width is 300px. If the value is false and the value in width option is set then:
        ///     The width of the grid is the width set in option.
        ///     The column width are not recalculated and have the values defined in colModel. (default: true)
        /// </summary>
        /// <param name = "shrinkToFit">Boolean indicating if shrink to fit is enforced</param>
        public JQGrid<TModel> SetShrinkToFit(bool shrinkToFit)
        {
            this.shrinkToFit = shrinkToFit;
            return this;
        }

        /// <summary>
        ///     Sets direction in which sort icons are displayed (default: vertical)
        /// </summary>
        /// <param name = "sortIconDirection">Direction in which sort icons are displayed</param>
        public JQGrid<TModel> SetSortIconDirection(Direction sortIconDirection)
        {
            this.sortIconDirection = sortIconDirection;
            return this;
        }

        /// <summary>
        ///     The initial sorting name when we use datatypes xml or json (data returned from server).
        ///     This parameter is added to the url. If set and the index (name) matches the name from the
        ///     colModel then by default an image sorting icon is added to the column, according to
        ///     the parameter sortorder.
        /// </summary>
        /// <param name = "sortName"></param>
        public JQGrid<TModel> SetSortName(string sortName)
        {
            this.sortName = sortName;
            return this;
        }

        /// <summary>
        ///     If enabled columns are sorted when header is clicked (default: true)
        ///     Warning, if disabled and setShowAllSortIcons is set to false, sorting will
        ///     be effectively disabled
        /// </summary>
        /// <param name = "sortOnHeaderClick">Boolean indicating if columns will sort on headerclick</param>
        /// <returns></returns>
        public JQGrid<TModel> SetSortOnHeaderClick(bool sortOnHeaderClick)
        {
            this.sortOnHeaderClick = sortOnHeaderClick;
            return this;
        }

        /// <summary>
        ///     The initial sorting order when we use datatypes xml or json (data returned from server).
        ///     This parameter is added to the url. Two possible values - asc or desc. (default: asc)
        /// </summary>
        /// <param name = "sortOrder">Sortorder</param>
        public JQGrid<TModel> SetSortOrder(SortDirection sortOrder)
        {
            this.sortOrder = sortOrder;
            return this;
        }

        /// <summary>
        ///     Determines how to post the data on which we perform searching.
        ///     When the this option is false the posted data is in key:value pair, if the option is true, the posted data is equal on those as in searchGrid method.
        ///     See here: http://www.trirand.com/jqgridwiki/doku.php?id=wiki:advanced_searching#options
        ///     (default: true)
        /// </summary>
        /// <param name = "stringResult">Boolean indicating if</param>
        public JQGrid<TModel> SetStringResult(bool stringResult)
        {
            this.stringResult = stringResult;
            return this;
        }

        /// <summary>
        ///     This option enabled the toolbar of the grid.  When we have two toolbars (can be set using setToolbarPosition)
        ///     then two elements (div) are automatically created. The id of the top bar is constructed like
        ///     “t_”+id of the grid and the bottom toolbar the id is “tb_”+id of the grid. In case when
        ///     only one toolbar is created we have the id as “t_” + id of the grid, independent of where
        ///     this toolbar is created (top or bottom). You can use jquery to add elements to the toolbars.
        /// </summary>
        /// <param name = "toolbar">Boolean indicating if toolbar is enabled</param>
        public JQGrid<TModel> SetToolbar(bool toolbar)
        {
            this.toolbar = toolbar;
            return this;
        }

        /// <summary>
        ///     Sets toolbarposition (default: top)
        /// </summary>
        /// <param name = "toolbarPosition">Position of toolbar</param>
        public JQGrid<TModel> SetToolbarPosition(ToolbarPosition toolbarPosition)
        {
            this.toolbarPosition = toolbarPosition;
            return this;
        }

        /// <summary>
        ///     When enabled this option place a pager element at top of the grid below the caption
        ///     (if available). If another pager is defined both can coexists and are refreshed in sync.
        ///     (default: false)
        /// </summary>
        /// <param name = "topPager">Boolean indicating if toppager is enabled</param>
        public JQGrid<TModel> SetTopPager(bool topPager)
        {
            this.topPager = topPager;
            return this;
        }

        /// <summary>
        ///     The url of the file that holds the request
        /// </summary>
        /// <param name = "url">Data url</param>
        public JQGrid<TModel> SetUrl(string url)
        {
            this.url = url;
            return this;
        }

        /// <summary>
        ///     When set to true we directly place the user data array userData in the footer.
        ///     The rules are as follows: If the userData array contains a name which matches any name defined in colModel,
        ///     then the value is placed in that column. If there are no such values nothing is placed.
        ///     Note that if this option is used we use the current formatter options (if available) for that column.
        ///     (default: false)
        /// </summary>
        /// <param name = "userDataOnFooter">Boolean indicating whether user data is set on footer row</param>
        public JQGrid<TModel> SetUserDataOnFooter(bool userDataOnFooter)
        {
            this.userDataOnFooter = userDataOnFooter;
            return this;
        }

        /// <summary>
        ///     If true, jqGrid displays the beginning and ending record number in the grid,
        ///     out of the total number of records in the query.
        ///     This information is shown in the pager bar (bottom right by default)in this format:
        ///     “View X to Y out of Z”.
        ///     If this value is true, there are other parameters that can be adjusted,
        ///     including 'emptyrecords' and 'recordtext'. (default: false)
        /// </summary>
        /// <param name = "viewRecords">Boolean indicating if recordnumbers are shown in grid</param>
        public JQGrid<TModel> SetViewRecords(bool viewRecords)
        {
            this.viewRecords = viewRecords;
            return this;
        }

        /// <summary>
        /// Creates virtual scrolling grid. When enabled, the pager elements are disabled and we can use the vertical scrollbar to load data.
        /// </summary>
        /// <param name="virtualScroll">Enables virtual scrolling when set to true</param>
        /// <param name="justHoldVisibleLines">Boolean indicating if only the visible lines in the grid should be held in memory (prevents memory leaks)</param>
        /// <returns></returns>
        public JQGrid<TModel> SetVirtualScroll(bool virtualScroll, bool justHoldVisibleLines = true)
        {
            if (virtualScroll && justHoldVisibleLines)
            {
                scrollInt = 1;
            }
            else if (!virtualScroll)
            {
                scroll = false;
            }
            else
            {
                scroll = true;
            }

            return this;
        }

        /// <summary>
        ///     If this option is not set, the width of the grid is a sum of the widths of the columns
        ///     defined in the colModel (in pixels). If this option is set, the initial width of each
        ///     column is set according to the value of shrinkToFit option.
        /// </summary>
        /// <param name = "width">Width in pixels</param>
        public JQGrid<TModel> SetWidth(int width)
        {
            this.width = width;
            return this;
        }

        /// <summary>
        /// This option is set to true by default and means the grid will be wrapped in a container div and
        /// automatically resize when the window is resized.
        /// </summary>
        /// <param name="responsive"></param>
        /// <returns></returns>
        public JQGrid<TModel> SetResponsive(bool responsive)
        {
            this.responsive = responsive;
            return this;
        }

        /// <summary>
        /// The URL to which the buttons are to perform an HTTP post to.
        /// </summary>
        /// <param name="formActionUrl"></param>
        /// <returns></returns>
        public JQGrid<TModel> HasFormActionUrl(string formActionUrl)
        {
            this.formActionUrl = formActionUrl;
            return this;
        }

        #endregion Fluid Methods

        /// <summary>
        /// Renders the required Html Elements
        /// </summary>
        /// <returns></returns>
        public string RenderHtmlElements()
        {
            var sb = new StringBuilder();

            bool hasForm = !string.IsNullOrEmpty(formActionUrl);

            if (hasForm)
            {
                sb.AppendFormat("<form action=\"{0}\" method=\"post\" style=\"margin:0;\" data-ajax=\"true\" data-ajax-success=\"$('#{1}').trigger('reloadGrid')\" novalidate=\"novalidate\">", formActionUrl, clientId);
            }

            if (responsive)
            {
                sb.AppendFormat("<div id=\"{0}_Container\">", clientId);
            }

            sb.AppendFormat("<table id=\"{0}\"></table>", clientId);

            if (!this.pager.IsNullOrWhiteSpace())
            {
                sb.AppendFormat("<div id=\"{0}\"></div>", this.pager);
            }

            if (this.topPager == true)
            {
                sb.AppendFormat("<div id=\"{0}_toppager\"></div>", clientId);
            }

            if (responsive)
            {
                sb.Append("</div>");
            }

            if (hasForm)
            {
                sb.Append("</form>");
            }

            return sb.ToString();
        }

        public string RenderJavascript()
        {
            // Create javascript
            var script = new StringBuilder();

            // Start script
            if (asyncLoad.HasValue && asyncLoad.Value)
            {
                script.AppendLine("jQuery(window).ready(function () {");
            }
            else
            {
                script.AppendLine("jQuery(document).ready(function () {");
            }

            script.AppendLine("jQuery('#" + clientId + "').jqGrid({");

            // Make sure there is at most one key
            if (_columns.Count(r => r.IsKey) > 1)
            {
                throw new ArgumentException("Too many key columns added. Maximum allowed id 1.");
            }

            // Altrows
            if (altRows.HasValue)
            {
                script.AppendFormat("altRows:{0},", altRows.ToString().ToLower()).AppendLine();
            }

            // Altclass
            if (!altClass.IsNullOrWhiteSpace())
            {
                script.AppendFormat("altclass:'{0}',", altClass).AppendLine();
            }

            // Autoencode
            if (autoEncode.HasValue)
            {
                script.AppendFormat("autoencode:{0},", autoEncode.ToString().ToLower()).AppendLine();
            }

            // Autowidth
            if (autoWidth.HasValue)
            {
                script.AppendFormat("autowidth:{0},", autoWidth.ToString().ToLower()).AppendLine();
            }

            // Caption
            if (!caption.IsNullOrWhiteSpace())
            {
                script.AppendFormat("caption:'{0}',", caption).AppendLine();
            }

            // Datatype
            script.AppendLine(enabledTreeGrid
                ? string.Format("treedatatype:'{0}',", dataType.ToString().ToLower())
                : string.Format("datatype:'{0}',", dataType.ToString().ToLower()));

            if (dataType == DataType.Json && jsonReader != null)
            {
                script.AppendLine("jsonReader:{'repeatitems':" + jsonReader.RepeatItems.ToString().ToLower() + ", 'id': '" + jsonReader.Id + "' },");
            }

            // Emptyrecords
            if (!emptyRecords.IsNullOrWhiteSpace())
            {
                script.AppendFormat("emptyrecords:'{0}',", emptyRecords).AppendLine();
            }

            // FooterRow
            if (footerRow.HasValue)
            {
                script.AppendFormat("footerrow:{0},", footerRow.Value.ToString().ToLower()).AppendLine();

                // UserDataOnFooter
                if (userDataOnFooter.HasValue)
                {
                    script.AppendFormat("userDataOnFooter:{0},", footerRow.Value.ToString().ToLower()).AppendLine();
                }
            }

            // Forcefit
            if (forceFit.HasValue)
            {
                script.AppendFormat("forceFit:{0},", forceFit.ToString().ToLower()).AppendLine();
            }

            // Gridview
            if (gridView.HasValue)
            {
                script.AppendFormat("gridview:{0},", gridView.ToString().ToLower()).AppendLine();
            }

            // HeaderTitles
            if (headerTitles.HasValue)
            {
                script.AppendFormat("headertitles:{0},", headerTitles.ToString().ToLower()).AppendLine();
            }

            // Height (set 100% if no value is specified except when scroll is set to true otherwise layout is not as it is supposed to be)
            if (!height.HasValue)
            {
                if ((!scroll.HasValue || scroll == false) && !scrollInt.HasValue)
                {
                    script.AppendFormat("height:'{0}',", "100%").AppendLine();
                }
            }
            else
            {
                script.AppendFormat("height:{0},", height).AppendLine();
            }

            // Hiddengrid
            if (hiddenGrid.HasValue)
            {
                script.AppendFormat("hiddengrid:{0},", hiddenGrid.ToString().ToLower()).AppendLine();
            }

            // Hidegrid
            if (hideGrid.HasValue)
            {
                script.AppendFormat("hidegrid:{0},", hideGrid.ToString().ToLower()).AppendLine();
            }

            // HoverRows
            if (hoverRows.HasValue)
            {
                script.AppendFormat("hoverrows:{0},", hoverRows.ToString().ToLower()).AppendLine();
            }

            // Loadonce
            if (loadOnce.HasValue)
            {
                script.AppendFormat("loadonce:{0},", loadOnce.ToString().ToLower()).AppendLine();
            }

            // Loadtext
            if (!loadText.IsNullOrWhiteSpace())
            {
                script.AppendFormat("loadtext:'{0}',", loadText).AppendLine();
            }

            // LoadUi
            if (loadUI.HasValue)
            {
                script.AppendFormat("loadui:'{0}',", loadUI.ToString().ToLower()).AppendLine();
            }

            // MultiBoxOnly
            if (multiBoxOnly.HasValue)
            {
                script.AppendFormat("multiboxonly:{0},", multiBoxOnly.ToString().ToLower()).AppendLine();
            }

            // MultiKey
            if (multiKey.HasValue)
            {
                script.AppendFormat("multikey:'{0}',", multiKey.ToString().ToLower()).AppendLine();
            }

            // MultiSelect
            if (multiSelect.HasValue)
            {
                script.AppendFormat("multiselect:{0},", multiSelect.ToString().ToLower()).AppendLine();
            }

            // MultiSelectWidth
            if (multiSelectWidth.HasValue)
            {
                script.AppendFormat("multiselectWidth:{0},", multiSelectWidth).AppendLine();
            }

            // Page
            if (page.HasValue)
            {
                script.AppendFormat("page:{0},", page).AppendLine();
            }

            // Pager
            if (!pager.IsNullOrWhiteSpace())
            {
                script.AppendFormat("pager:'#{0}',", pager).AppendLine();
            }

            // PagerPos
            if (pagerPos.HasValue)
            {
                script.AppendFormat("pagerpos:'{0}',", pagerPos.ToString().ToLower()).AppendLine();
            }

            // PgButtons
            if (pgButtons.HasValue)
            {
                script.AppendFormat("pgbuttons:{0},", pgButtons.ToString().ToLower()).AppendLine();
            }

            // PgInput
            if (pgInput.HasValue)
            {
                script.AppendFormat("pginput:{0},", pgInput.ToString().ToLower()).AppendLine();
            }

            // PGText
            if (!pgText.IsNullOrWhiteSpace())
            {
                script.AppendFormat("pgtext:'{0}',", pgText).AppendLine();
            }

            //postData
            if (!postData.IsNullOrEmpty())
            {
                var sb = new StringBuilder();

                foreach (var keyValue in postData)
                {
                    sb.AppendFormat("'{0}': {1},", keyValue.Key, keyValue.Value);
                }
                sb.Remove(sb.Length - 1, 1);

                script.AppendFormat("postData: {{ {0} }},", sb.ToString()).AppendLine();
            }

            // RecordPos
            if (recordPos.HasValue)
            {
                script.AppendFormat("recordpos:'{0}',", recordPos.ToString().ToLower()).AppendLine();
            }

            // RecordText
            if (!recordText.IsNullOrWhiteSpace())
            {
                script.AppendFormat("recordtext:'{0}',", recordText).AppendLine();
            }

            // Request Type
            if (requestType.HasValue)
            {
                script.AppendFormat("mtype:'{0}',", requestType.ToString().ToLower()).AppendLine();
            }

            // ResizeClass
            if (!resizeClass.IsNullOrWhiteSpace())
            {
                script.AppendFormat("resizeclass:'{0}',", resizeClass).AppendLine();
            }

            // Rowlist
            if (rowList != null)
            {
                script.AppendFormat(
                    "rowList:[{0}],",
                    string.Join(",", ((from p in rowList select p.ToString()).ToArray()))).AppendLine();
            }

            // Rownum
            if (rowNum.HasValue)
            {
                script.AppendFormat("rowNum:{0},", rowNum).AppendLine();
            }

            // Rownumbers
            if (rowNumbers.HasValue)
            {
                script.AppendFormat("rownumbers:{0},", rowNumbers.ToString().ToLower()).AppendLine();
            }

            // RowNumWidth
            if (rowNumWidth.HasValue)
            {
                script.AppendFormat("rownumWidth:{0},", rowNumWidth).AppendLine();
            }

            // Virtual scroll (make sure either scroll or scrollint is set, never both)
            if (scrollInt.HasValue)
            {
                script.AppendFormat("scroll:{0},", scrollInt).AppendLine();
            }
            else if (scroll.HasValue)
            {
                script.AppendFormat("scroll:{0},", scroll.ToString().ToLower()).AppendLine();
            }

            // ScrollOffset
            if (scrollOffset.HasValue)
            {
                script.AppendFormat("scrollOffset:{0},", scrollOffset).AppendLine();
            }

            // ScrollRows
            if (scrollRows.HasValue)
            {
                script.AppendFormat("scrollrows:{0},", scrollRows.ToString().ToLower()).AppendLine();
            }

            // ScrollTimeout
            if (scrollTimeout.HasValue)
            {
                script.AppendFormat("scrollTimeout:{0},", scrollTimeout).AppendLine();
            }

            // Sortname
            if (!sortName.IsNullOrWhiteSpace())
            {
                script.AppendFormat("sortname:'{0}',", sortName).AppendLine();
            }

            // Sorticons
            if (showAllSortIcons.HasValue || sortIconDirection.HasValue || sortOnHeaderClick.HasValue)
            {
                // Set defaults
                if (!showAllSortIcons.HasValue)
                {
                    showAllSortIcons = false;
                }

                if (!sortIconDirection.HasValue)
                {
                    sortIconDirection = Direction.Vertical;
                }

                if (!sortOnHeaderClick.HasValue)
                {
                    sortOnHeaderClick = true;
                }

                script.AppendFormat(
                    "viewsortcols:[{0},'{1}',{2}],",
                    showAllSortIcons.ToString().ToLower(),
                    sortIconDirection.ToString().ToLower(),
                    sortOnHeaderClick.ToString().ToLower()).AppendLine();
            }

            // Shrink to fit
            if (shrinkToFit.HasValue)
            {
                script.AppendFormat("shrinkToFit:{0},", shrinkToFit.ToString().ToLower()).AppendLine();
            }

            // Sortorder
            if (sortOrder.HasValue)
            {
                script.AppendFormat("sortorder:'{0}',", sortOrder.ToString().ToLower()).AppendLine();
            }

            // Toolbar
            if (toolbar.HasValue)
            {
                script.AppendFormat("toolbar:[{0},'{1}'],", toolbar.ToString().ToLower(), toolbarPosition.ToString().ToLower()).AppendLine();
            }

            // Toppager
            if (topPager.HasValue)
            {
                script.AppendFormat("toppager:{0},", topPager.ToString().ToLower()).AppendLine();
            }

            // Url
            if (!url.IsNullOrWhiteSpace())
            {
                script.AppendFormat("url:'{0}',", url).AppendLine();
            }

            // View records
            if (viewRecords.HasValue)
            {
                script.AppendFormat("viewrecords:{0},", viewRecords.ToString().ToLower()).AppendLine();
            }

            // Width
            if (width.HasValue)
            {
                script.AppendFormat("width:'{0}',", width).AppendLine();
            }

            // IgnoreCase
            if (ignoreCase.HasValue)
            {
                script.AppendFormat("ignoreCase:{0},", ignoreCase.ToString().ToLower()).AppendLine();
            }

            // onAfterInsertRow
            if (!onAfterInsertRow.IsNullOrWhiteSpace())
            {
                script.AppendFormat("afterInsertRow: function(rowid, rowdata, rowelem) {{{0}}},", onAfterInsertRow).AppendLine();
            }

            // jqGrid Hacking detected beause jqGrid didn't implement default search value correctly we gonna fix this in here
            if (_columns.Any(x => x.HasDefaultSearchValue))
            {
                #region jqGrid javascript onbefore request hack

                var defaultValueColumns = _columns.Where(x => x.HasDefaultSearchValue).Select(x => new { field = x.Index, op = x.SearchOption, data = x.DefaultSearchValue });

                var onbeforeRequestHack =
@"function() {
    var defaultValueColumns = " + new JavaScriptSerializer().Serialize(defaultValueColumns) + @";
    var colModel = this.p.colModel;

    if (defaultValueColumns.length > 0) {
        var postData = this.p.postData;

        var filters = {};

        if (postData.hasOwnProperty('filters')) {
            filters = JSON.parse(postData.filters);
        }

        var rules = [];

        if (filters.hasOwnProperty('rules')) {
            rules = filters.rules;
        }

        $.each(defaultValueColumns, function (defaultValueColumnIndex, defaultValueColumn) {
            $.each(rules, function (index, rule) {
                if (defaultValueColumn.field == rule.field) {
                    delete rules[index];
                    return;
                }
            });

            rules.push(defaultValueColumn);
        });

        filters.groupOp = 'AND';
        filters.rules = rules;

        postData._search = true;
        postData.filters = JSON.stringify(filters);

        $(this).setGridParam({ search: true, 'postData': postData});
    }

    this.p.beforeRequest = function() { " + ((!onBeforeRequest.IsNullOrWhiteSpace()) ? onBeforeRequest : "") + @" };
    this.p.beforeRequest.call(this);
} ";

                #endregion jqGrid javascript onbefore request hack

                script.AppendFormat("beforeRequest: {0},", onbeforeRequestHack).AppendLine();
            }
            // onBeforeRequest
            else if (!onBeforeRequest.IsNullOrWhiteSpace())
            {
                script.AppendFormat("beforeRequest: function() {{{0}}},", onBeforeRequest).AppendLine();
            }

            // onBeforeSelectRow
            if (!onBeforeSelectRow.IsNullOrWhiteSpace())
            {
                script.AppendFormat("beforeSelectRow: function(rowid, e) {{{0}}},", onBeforeSelectRow).AppendLine();
            }

            // onGridComplete
            if (!onGridComplete.IsNullOrWhiteSpace())
            {
                script.AppendFormat("gridComplete: function() {{{0}}},", onGridComplete).AppendLine();
            }

            // onLoadBeforeSend
            if (!onLoadBeforeSend.IsNullOrWhiteSpace())
            {
                script.AppendFormat("loadBeforeSend: function(xhr, settings) {{{0}}},", onLoadBeforeSend).AppendLine();
            }

            // onLoadComplete
            if (!onLoadComplete.IsNullOrWhiteSpace())
            {
                script.AppendFormat("loadComplete: function(xhr) {{{0}}},", onLoadComplete).AppendLine();
            }

            // onLoadError
            if (!onLoadError.IsNullOrWhiteSpace())
            {
                script.AppendFormat("loadError: function(xhr, status, error) {{{0}}},", onLoadError).AppendLine();
            }

            // onCellSelect
            if (!onCellSelect.IsNullOrWhiteSpace())
            {
                script.AppendFormat("onCellSelect: function(rowid, iCol, cellcontent, e) {{{0}}},", onCellSelect).AppendLine();
            }

            // onDblClickRow
            if (!onDblClickRow.IsNullOrWhiteSpace())
            {
                script.AppendFormat("ondblClickRow: function(rowid, iRow, iCol, e) {{{0}}},", onDblClickRow).AppendLine();
            }

            // onHeaderClick
            if (!onHeaderClick.IsNullOrWhiteSpace())
            {
                script.AppendFormat("onHeaderClick: function(gridstate) {{{0}}},", onHeaderClick).AppendLine();
            }

            // onPaging
            if (!onPaging.IsNullOrWhiteSpace())
            {
                script.AppendFormat("onPaging: function(pgButton) {{{0}}},", onPaging).AppendLine();
            }
            // onRightClickRow
            if (!onRightClickRow.IsNullOrWhiteSpace())
            {
                script.AppendFormat("onRightClickRow: function(rowid, iRow, iCol, e) {{{0}}},", onRightClickRow).AppendLine();
            }

            // onSelectAll
            if (!onSelectAll.IsNullOrWhiteSpace())
            {
                script.AppendFormat("onSelectAll: function(aRowids, status) {{{0}}},", onSelectAll).AppendLine();
            }

            // onSelectRow event
            if (!onSelectRow.IsNullOrWhiteSpace())
            {
                script.AppendFormat("onSelectRow: function(rowid, status) {{{0}}},", onSelectRow).AppendLine();
            }
            // onSortCol
            if (!onSortCol.IsNullOrWhiteSpace())
            {
                script.AppendFormat("onSortCol: function(index, iCol, sortorder) {{{0}}},", onSortCol).AppendLine();
            }

            // onResizeStart
            if (!onResizeStart.IsNullOrWhiteSpace())
            {
                script.AppendFormat("resizeStart: function(jqGridEvent, index) {{{0}}},", onResizeStart).AppendLine();
            }

            // onResizeStop
            if (!onResizeStop.IsNullOrWhiteSpace())
            {
                script.AppendFormat("resizeStop: function(newwidth, index) {{{0}}},", onResizeStop).AppendLine();
            }

            // onSerializeGridData
            if (!onSerializeGridData.IsNullOrWhiteSpace())
            {
                script.AppendFormat("serializeGridData: function(postData) {{{0}}},", onSerializeGridData).AppendLine();
            }

            // TreeGrid controls
            if (enabledTreeGrid)
            {
                if (_columns.Count(r => r.IsExpandable) > 1)
                {
                    throw new ArgumentException("Too many key columns set as expandable. Maximum allowed is 1.");
                }

                var keyColumn = _columns.FirstOrDefault(r => r.IsKey);
                var expandableColumn = _columns.FirstOrDefault(r => r.IsExpandable);

                if (keyColumn == null)
                {
                    throw new ArgumentException("Enabling treegrid needs at least one column set as Key.");
                }

                if (expandableColumn == null)
                {
                    throw new ArgumentException("Enabling treegrid needs at least one column set as expandable.");
                }

                script.AppendLine("treeGrid: true,");
                script.AppendFormat("ExpandColumn : '{0}',", expandableColumn.Name).AppendLine();
                script.AppendFormat("treeGridModel : '{0}',", treeGridModel.ToString().ToLower()).AppendLine();
                script.AppendFormat("tree_root_level : {0},", treeGridRootLevel).AppendLine();
            }

            // Colmodel
            script.AppendLine("colModel: [");
            var colModel = string.Join(",", ((from c in _columns select c.ToString()).ToArray()));
            script.AppendLine(colModel);
            script.AppendLine("]");

            // End jqGrid call
            script.AppendLine("});");

            if (searchToolbar == true && !pager.IsNullOrWhiteSpace() &&
                (searchClearButton.HasValue && searchClearButton == true || searchToggleButton.HasValue && searchToggleButton == true))
            {
                script.AppendLine(
                    "jQuery('#" + clientId + "').jqGrid('navGrid',\"#" + pager + "\",{edit:false,add:false,del:false,search:false,refresh:false}); ");
            }

            // Search clear button
            if (searchToolbar == true && searchClearButton.HasValue && !pager.IsNullOrWhiteSpace() &&
                searchClearButton == true)
            {
                script.AppendLine("jQuery('#" + clientId + "').jqGrid('navButtonAdd',\"#" + pager +
                                 "\",{caption:\"Clear\",title:\"Clear Search\",buttonicon :'ui-icon-refresh', onClickButton:function(){jQuery('#" + clientId + "')[0].clearToolbar(); }}); ");
            }

            if (searchToolbar == true && searchToggleButton.HasValue && !pager.IsNullOrWhiteSpace() && searchToggleButton == true)
            {
                script.AppendLine("jQuery('#" + clientId + "').jqGrid('navButtonAdd',\"#" + pager +
                              "\",{caption:\"Toggle Search\",title:\"Toggle Search\",buttonicon :'ui-icon-refresh', onClickButton:function(){jQuery('#" + clientId + "')[0].toggleToolbar(); }}); ");
            }

            // Search toolbar
            if (searchToolbar == true)
            {
                script.Append("jQuery('#" + clientId + "').jqGrid('filterToolbar', {stringResult:" + stringResult.ToString().ToLower());

                if (searchOnEnter.HasValue)
                {
                    script.AppendFormat(", searchOnEnter:{0}", searchOnEnter.ToString().ToLower());
                }

                script.AppendLine("});");
            }

            // End script
            script.AppendLine("});");

            // Insert grid id where needed (in columns)
            script.Replace("##gridid##", clientId);

            if (responsive)
            {
                script.AppendFormat(
@"$(document).ready(function () {{
    $(window).resize(function () {{
        var width = $('#{0}_Container').width();
        $('#{0}').setGridWidth(width);
    }})
}});", clientId);
            }

            return script.ToString();
        }

        public string ToHtmlString()
        {
            return ToString();
        }

        /// <summary>
        ///     Creates and returns javascript + required html elements to render grid
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var scriptRegister = new ScriptRegister(workContext);
            scriptRegister.IncludeInline(RenderJavascript().Replace("##gridid##", clientId));

            // Return script + required elements
            return RenderHtmlElements();
        }
    }
}