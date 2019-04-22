var pager; // declare the pager variable outside
$(function () {
    pager = new Pager('tabledata', 5);
    pager.init();
    pager.showPageNav('pager', 'pageNavPosition');
    pager.showPage(1);
});
function Pager(tableName, itemsPerPage) {
    this.tableName = tableName;
    this.itemsPerPage = itemsPerPage;
    this.currentPage = 1;
    this.pages = 0;
    this.inited = false;
    this.showRecords = function (from, to) {
        var rows = document.getElementById(tableName).rows;
        // i starts from 1 to skip table header row
        for (var i = 1; i < rows.length; i++) {
            if (i < from || i > to)
                rows[i].style.display = 'none';
            else
                rows[i].style.display = '';
        }
    }
    this.showPage = function (pageNumber) {
        if (!this.inited) {
            alert("not inited");
            return;
        }
        var oldPageAnchor = document.getElementById('pg' + this.currentPage);
        oldPageAnchor.className = 'pg-normal';
        this.currentPage = pageNumber;
        var newPageAnchor = document.getElementById('pg' + this.currentPage);
        newPageAnchor.className = 'pg-selected';
        var from = (pageNumber - 1) * itemsPerPage + 1;
        var to = from + itemsPerPage - 1;
        this.showRecords(from, to);
    }
    this.prev = function () {
        if (this.currentPage > 1)
            this.showPage(this.currentPage - 1);
    }
    this.next = function () {
        if (this.currentPage < this.pages) {
            this.showPage(this.currentPage + 1);
        }
    }
    this.init = function () {
        var rows = document.getElementById(tableName).rows;
        var records = (rows.length - 1);
        this.pages = Math.ceil(records / itemsPerPage);
        this.inited = true;
    }
    this.showPageNav = function (pagerName, positionId) {
        if (!this.inited) {
            alert("not inited");
            return;
        }
        var element = document.getElementById(positionId);
        var pagerHtml = '<span onclick="' + pagerName + '.prev();" class="pg-normal"> &#171 Edellinen </span> | ';
        for (var page = 1; page <= this.pages; page++)
            pagerHtml += '<span id="pg' + page + '" class="pg-normal" onclick="' + pagerName + '.showPage(' + page + ');">' + page + '</span> | ';
        pagerHtml += '<span onclick="' + pagerName + '.next();" class="pg-normal"> Seuraava &#187;</span>';
        element.innerHTML = pagerHtml;
    }
}

//function myFunction() {
//    // Declare variables
//    var input, filter, table, tr, td, i, txtValue;
//    input = document.getElementById("myInput");
//    filter = input.value.toUpperCase();
//    table = document.getElementById("tabledata");
//    tr = table.getElementsByTagName("tr");
//    // Loop through all table rows, and hide those who don't match the search query
//    for (i = 0; i < tr.length; i++) {
//        td = tr[i].getElementsByTagName("td")[3];
//        if (td) {
//            txtValue = td.textContent || td.innerText;
//            if (txtValue.toUpperCase().indexOf(filter) > -1) {
//                tr[i].style.display = "";
//            } else {
//                tr[i].style.display = "none";
//            }
//        }
//    }
//}
