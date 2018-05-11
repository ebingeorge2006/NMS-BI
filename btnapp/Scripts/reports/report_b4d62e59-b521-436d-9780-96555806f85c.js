var reportsAndVisuals = [
    { reportName: 'ReportSection9a26698070db0c678c17', reportAlias: 'Summary', visualName: 'e6f0a25e81434bac3d0b' },
    { reportName: 'ReportSection8ace1946126730501097', reportAlias: 'Hotel Details', visualName: '662140001cd759073b0d' },
    { reportName: 'ReportSectionf9122c9f0517cd8707c4', reportAlias: 'Car Details', visualName: '51fb1a2e89534d957310' },
    { reportName: 'ReportSection4af9717a16b38e3a54ba', reportAlias: 'Meal Details', visualName: '350444d0853509ccbc0c' }
];


var Export_Data = function (selectedFileType) {

    console.log('exporting data...');
    // Get models. models contains enums that can be used.
    var models = window['powerbi-client'].models;

    // Get a reference to the embedded report HTML element
    var embedContainer = $('#reportContainer')[0];

    // Get a reference to the embedded report.
    report = powerbi.get(embedContainer);

    // Retrieve the page collection and get the visuals for the first page.
    report.getPages()
        .then(function (pages) {

            // Retrieve active page.
            var activePage = pages.find(function (page) {
                return page.isActive
            });

            var reportVisual = _.find(reportsAndVisuals, function (rv) {
                return rv.reportName === activePage.name;
            });

            var reportName = reportVisual.visualName;
            var pageName = activePage.name;

            activePage.getVisuals()
                .then(function (visuals) {

                    // Retrieve the wanted visual.
                    var visual = visuals.find(function (visual) {
                        return visual.name === reportName;
                    });

                    // Exports visual data
                    visual.exportData(models.ExportDataType.Summarized)
                        .then(function (data) {

                            var newLineChar = /\n/;
                            var headAndData = data.data.split(newLineChar);

                            // var columnNameString = data.data.substr(0, data.data.search(newLineChar));
                            /* Get Column Names */
                            var ColumNames = headAndData[0].splitComma();



                            var rows = headAndData.slice(1);
                            var dataRows = _.map(rows, function (val) { return val.splitComma(); });


                            ga('send', {
                                hitType: 'event',
                                eventCategory: 'Export',
                                eventAction: 'ExportTo' + selectedFileType,
                                eventLabel: reportName,
                                fieldsObject: {
                                    PageName: pageName,
                                    ReportName: reportName
                                }
                            });

                            var obj = {
                                hitType: 'event',
                                eventCategory: 'Export',
                                eventAction: 'ExportTo' + selectedFileType,
                                eventLabel: reportName
                            }

                            if ($('#toggleNotifications').is(":checked")) {
                                alertify.success((library.json.prettyPrint(obj)));
                            }



                            switch (selectedFileType) {

                                case "PDF":
                                    switch (reportName) {
                                        case "e6f0a25e81434bac3d0b":
                                            exportPDFTable(ColumNames, dataRows, reportName);
                                            break;

                                        default:
                                            exportPDFPivoted(ColumNames, dataRows, reportName);
                                            break;
                                    }

                                    break;

                                case "CSV":
                                    exportTableToCSVColsRows(ColumNames, dataRows, 'report.csv');
                                    break;

                                default:
                                    exportPDF(ColumNames, dataRows, 'report.pdf');
                                    break;
                            }


                        })
                        .catch(function (errors) {
                            alertify.error(errors.message);
                        });
                })
                .catch(function (errors) {
                    alertify.error(errors.message);
                });
        })
        .catch(function (errors) {
            alertify.error(errors.message);
        });
}

exportToPDF = function () {
    Export_Data('PDF');
}

exportToCSV = function () {
    Export_Data('CSV');
}

getColumnArray2 = function (reportName, columns) {
    var result = [];
    switch (reportName) {
        case "51fb1a2e89534d957310":
            result = columns.slice(2);
            break;

        case "662140001cd759073b0d":
            result = columns.slice(1, columns.length - 1);
            break;

        case "350444d0853509ccbc0c":
            result = columns.slice(2);
            break;

        default:
            result = columns.slice(1, columns.length - 1);
            break;
    }

    return result;
}

getPivotColumn = function (reportName, columnArray, groupName) {
    var pivotColumn = {};


    switch (reportName) {
        case "51fb1a2e89534d957310":
            pivotColumn = {
                cost: columnArray[0],
                taxes: columnArray[1],
                total: columnArray[2],
                yoy: columnArray[3],
                group: groupName
            };
            break;

        case "662140001cd759073b0d":
            pivotColumn = {
                cost: columnArray[0],
                taxes: columnArray[1],
                total: columnArray[2],
                yoy: columnArray[3],
                group: groupName
            };
            break;

        case "350444d0853509ccbc0c":
            pivotColumn = {
                cost: columnArray[0],
                yoy: columnArray[1],
                group: groupName
            };
            break;

        default:
            pivotColumn = {
                cost: columnArray[0],
                taxes: columnArray[1],
                total: columnArray[2],
                yoy: columnArray[3],
                group: groupName
            };
            break;
    }

    return pivotColumn;
}

getFoundPivotColumn = function (reportName, columnArray, groupName) {
    var pivotColumn = {};


    switch (reportName) {
        case "51fb1a2e89534d957310":
            pivotColumn = {
                cost: columnArray[2],
                taxes: columnArray[3],
                total: columnArray[4],
                yoy: columnArray[5],
                group: groupName
            };
            break;

        case "662140001cd759073b0d":
            pivotColumn = {
                cost: columnArray[1],
                taxes: columnArray[2],
                total: columnArray[3],
                yoy: columnArray[4],
                group: groupName
            };
            break;

        case "350444d0853509ccbc0c":
            pivotColumn = {
                cost: columnArray[2],
                yoy: columnArray[3],
                group: groupName
            };
            break;

        default:
            pivotColumn = {
                cost: columnArray[1],
                taxes: columnArray[2],
                total: columnArray[3],
                yoy: columnArray[4],
                group: groupName
            };
            break;
    }

    return pivotColumn;
}

getCalulatedPivotColumn = function (reportName, dataArray, groupName) {
    var pivotColumn = {};


    switch (reportName) {
        case "51fb1a2e89534d957310":
            pivotColumn = {
                cost: '$' + (_.reduce(dataArray, function (memo, num) { return (memo + Number(num.cost.replace(/[^0-9\.-]+/g, ""))); }, 0) / dataArray.length).toFixed(2).toString(),
                taxes: '$' + (_.reduce(dataArray, function (memo, num) { return (memo + Number(num.taxes.replace(/[^0-9\.-]+/g, ""))); }, 0) / dataArray.length).toFixed(2).toString(),
                total: '$' + (_.reduce(dataArray, function (memo, num) { return (memo + Number(num.total.replace(/[^0-9\.-]+/g, ""))); }, 0) / dataArray.length).toFixed(2).toString(),
                yoy: (_.reduce(dataArray, function (memo, num) { return (memo + Number(num.yoy.replace(/[^0-9\.-]+/g, ""))); }, 0) / dataArray.length).toFixed(2).toString() + '%',
                group: groupName
            }
            break;

        case "662140001cd759073b0d":
            pivotColumn = {
                cost: '$' + (_.reduce(dataArray, function (memo, num) { return (memo + Number(num.cost.replace(/[^0-9\.-]+/g, ""))); }, 0) / dataArray.length).toFixed(2).toString(),
                taxes: '$' + (_.reduce(dataArray, function (memo, num) { return (memo + Number(num.taxes.replace(/[^0-9\.-]+/g, ""))); }, 0) / dataArray.length).toFixed(2).toString(),
                total: '$' + (_.reduce(dataArray, function (memo, num) { return (memo + Number(num.total.replace(/[^0-9\.-]+/g, ""))); }, 0) / dataArray.length).toFixed(2).toString(),
                yoy: (_.reduce(dataArray, function (memo, num) { return (memo + Number(num.yoy.replace(/[^0-9\.-]+/g, ""))); }, 0) / dataArray.length).toFixed(2).toString() + '%',
                group: groupName
            }
            break;

        case "350444d0853509ccbc0c":
            pivotColumn = {
                cost: '$' + (_.reduce(dataArray, function (memo, num) { return (memo + Number(num.cost.replace(/[^0-9\.-]+/g, ""))); }, 0) / dataArray.length).toFixed(2).toString(),
                yoy: (_.reduce(dataArray, function (memo, num) { return (memo + Number(num.yoy.replace(/[^0-9\.-]+/g, ""))); }, 0) / dataArray.length).toFixed(2).toString() + '%',
                group: groupName
            }
            break;

        default:
            pivotColumn = {
                cost: '$' + (_.reduce(dataArray, function (memo, num) { return (memo + Number(num.cost.replace(/[^0-9\.-]+/g, ""))); }, 0) / dataArray.length).toFixed(2).toString(),
                taxes: '$' + (_.reduce(dataArray, function (memo, num) { return (memo + Number(num.taxes.replace(/[^0-9\.-]+/g, ""))); }, 0) / dataArray.length).toFixed(2).toString(),
                total: '$' + (_.reduce(dataArray, function (memo, num) { return (memo + Number(num.total.replace(/[^0-9\.-]+/g, ""))); }, 0) / dataArray.length).toFixed(2).toString(),
                yoy: (_.reduce(dataArray, function (memo, num) { return (memo + Number(num.yoy.replace(/[^0-9\.-]+/g, ""))); }, 0) / dataArray.length).toFixed(2).toString() + '%',
                group: groupName
            }
            break;
    }

    return pivotColumn;
}

getColumnGroupPosition = function (reportName, columns) {
    var result;
    switch (reportName) {
        case "51fb1a2e89534d957310":
            result = 1
            break;

        case "662140001cd759073b0d":
            result = columns.length - 1;
            break;

        case "350444d0853509ccbc0c":
            result = 1;
            break;

        default:
            result = columns.length - 1;
            break;
    }

    return result;
}

exportPDFTable = function (columns, rows, reportName) {

    var exportedData = document.createElement('TABLE');
    exportedData.setAttribute('id', 'exportedData');
    exportedData.setAttribute('width', '2000px');
    document.getElementById('dvExportedData').innerHTML = exportedData.outerHTML;
    var table = document.getElementById("exportedData");

    var styleNormal = "font-family: Calibri; font-size: 24px; height: 25px;";
    var styleHeader = "padding: 2px 6px 3px 5px; color: rgb(255, 255, 255); background-color: rgb(22, 55, 90); box-shadow: black 0px 0px 0px 0px inset, rgb(179, 179, 179) -1px 0px 0px 0px inset, rgb(214, 214, 214) 0px -1px 0px 0px inset, black 0px 0px 0px 0px inset; box-sizing: border-box; height: 19px;";

    var row = table.insertRow(-1);
    for (var i = 0; i < columns.length; i++) {
        var headerCell = document.createElement("TH");
        headerCell.innerHTML = "<b>" + columns[i] + "</b>";
        if (i === 0) {
            styleHeader = styleHeader + "text-align: left;"
        }
        else {
            styleHeader = styleHeader + "text-align: center;"
        }

        headerCell.setAttribute("style", styleNormal + styleHeader);
        row.appendChild(headerCell);
    }

    for (var i = 0; i < rows.length; i++) {
        var row = table.insertRow(-1);


        if (i % 2 != 0 && i != rows.length - 1) {
            row.setAttribute("style", "background-color: #dddddd;");
        }

        for (var j = 0; j < rows[i].length; j++) {
            var dataCell = document.createElement("TD");
            dataCell.innerHTML = rows[i][j].replace(/"/g, '');

            if (j > 0) {
                styleNormal = styleNormal + 'text-align: center;'
            }
            else {
                styleNormal = styleNormal + 'text-align: left;'
            }

            if (rows[i][j].indexOf('-') > -1) {
                dataCell.setAttribute("style", styleNormal + "color: red;");
            }
            else {
                dataCell.setAttribute("style", styleNormal);
            }

            row.appendChild(dataCell);
        }

    }

    $('#hdnHtmlCode').val($('#dvExportedData').html());
    document.getElementById('CreatePDFDocument').click();

}

exportPDFPivoted = function (columns, rows, reportName) {



    rows = _.filter(rows, function (num) { return num.length > 1; });
    var rowsGroup = _.groupBy(rows, function (p) { return p[0]; });
    var colGroup = _.groupBy(rows, function (c) { return c[getColumnGroupPosition(reportName, columns)]; });

    var startHeadRow1 = {
        text: '',
        isAdded: false
    }, startHeadRow2 = {
        ext: '',
        isAdded: false
    };
    var colArray1 = [], colArray2 = [];
    var tableArray = [];


    startHeadRow1.text = columns[getColumnGroupPosition(reportName, columns)];
    startHeadRow2.text = columns[0];

    _(colGroup).each(function (elem, key) {
        if (!startHeadRow1.isAdded) {
            colArray1.push(startHeadRow1.text);
            startHeadRow1.isAdded = true;
        }
        colArray1.push(key);
    });
    colArray1.push('Total');

    tableArray.push(colArray1);


    colArray2 = getColumnArray2(reportName, columns); //columns.slice(1, columns.length - 1);

    var pivotColumn = getPivotColumn(reportName, colArray2, '');

    var tempArray = [];
    _(colGroup).each(function (elem, key) {
        if (!startHeadRow2.isAdded) {
            tempArray.push(startHeadRow2.text);
            startHeadRow2.isAdded = true;
        }
        tempArray.push(pivotColumn);
    });
    tempArray.push(pivotColumn); //for totals

    tableArray.push(tempArray);

    var rowGroupArray = [];
    _(rowsGroup).each(function (elem, key) {
        rowGroupArray.push(elem)
    });



    tempArray = [];
    var colTitleArray = colArray1.slice(1, colArray1.length - 1);
    for (var i = 0; i < rowGroupArray.length; i++) {
        tempArray = [];
        tempArray.push(rowGroupArray[i][0][0]);
        for (j = 0; j < colTitleArray.length; j++) {

            var found = _.find(rowGroupArray[i], function (obj) { return obj[getColumnGroupPosition(reportName, obj)] == colTitleArray[j] });
            if (found === undefined) {
                pivotColumn = getFoundPivotColumn(reportName, ['', '', '', '', ''], colTitleArray[j]);
            }
            else {
                pivotColumn = getFoundPivotColumn(reportName, found, colTitleArray[j]);
            }


            tempArray.push(pivotColumn);
        }
        var dataArray = tempArray.slice(1);

        pivotColumn = getCalulatedPivotColumn(reportName, dataArray, '');
        tempArray.push(pivotColumn);
        tableArray.push(tempArray);
    }

    tempArray = [];
    tempArray.push('Total');

    var colGroup1 = colArray1.slice(1, colArray1.length - 1);
    var tableDataArray = tableArray.slice(2);



    for (var i = 1; i <= colArray1.slice(1).length; i++) {
        dataArray = [];
        for (var j = 0; j < tableDataArray.length; j++) {

            var pivotColumn = {};
            if (tableDataArray[j][i].taxes === undefined || tableDataArray[j][i].total == undefined) {
                var tdArray = [
                    tableDataArray[j][i].cost,
                    tableDataArray[j][i].yoy
                ]
                pivotColumn = getPivotColumn(reportName, tdArray, '');
            }
            else {
                var tdArray = [
                    tableDataArray[j][i].cost,
                    tableDataArray[j][i].taxes,
                    tableDataArray[j][i].total,
                    tableDataArray[j][i].yoy
                ]
                pivotColumn = getPivotColumn(reportName, tdArray, '');
            }
            dataArray.push(pivotColumn);
        }
        var pivotColumn = getCalulatedPivotColumn(reportName, dataArray, '');

        tempArray.push(pivotColumn);
    }

    tableArray.push(tempArray);




    var exportedData = document.createElement('TABLE');
    exportedData.setAttribute('id', 'exportedData');
    exportedData.setAttribute('width', '2000px');
    document.getElementById('dvExportedData').innerHTML = exportedData.outerHTML;
    var table = document.getElementById("exportedData");

    var styleNormal = "font-family: Calibri; font-size: 24px; height: 25px;";
    var styleHeader = "padding: 2px 6px 3px 5px; color: rgb(255, 255, 255); background-color: rgb(22, 55, 90); box-shadow: black 0px 0px 0px 0px inset, rgb(179, 179, 179) -1px 0px 0px 0px inset, rgb(214, 214, 214) 0px -1px 0px 0px inset, black 0px 0px 0px 0px inset; box-sizing: border-box; height: 19px;";

    for (var i = 0; i < tableArray.length; i++) {
        var row = table.insertRow(-1);


        if (i % 2 != 0) {
            row.setAttribute("style", "background-color: #dddddd;");
        }

        for (var j = 0; j < tableArray[i].length; j++) {

            if (i == 1 && j == 0) {
                styleHeader = styleHeader + "text-align: left;"
            }
            else {
                styleHeader = styleHeader + "text-align: center;"
            }

            if (j == tableArray[i].length - 1 || i == tableArray.length) {
                styleHeader = styleHeader + "font-weight: bold;"
            }

            if (i <= 1) {

                if (i == 0 && j == 0) {
                    var headerCell = document.createElement("TH");
                    headerCell.innerHTML = "<b>" + tableArray[i][j].replace(/ /g, '-'); + "</b>";

                    headerCell.setAttribute("style", styleNormal + styleHeader);
                    row.appendChild(headerCell);
                }

                if (i == 0 && j > 0) {
                    var headerCell = document.createElement("TH");
                    headerCell.setAttribute('colspan', colArray2.length);
                    headerCell.setAttribute("style", styleNormal + styleHeader);
                    headerCell.innerHTML = "<b>" + tableArray[i][j].replace(/ /g, '-'); + "</b>";
                    row.appendChild(headerCell);
                }

                if (i == 1 && j == 0) {
                    var headerCell = document.createElement("TH");
                    headerCell.innerHTML = tableArray[i][j].replace(/ /g, '-');;
                    headerCell.setAttribute("style", styleNormal + styleHeader);
                    row.appendChild(headerCell);
                }

                if (i == 1 && j > 0) {
                    var headerCell = document.createElement("TH");
                    headerCell.innerHTML = "Cost";
                    headerCell.setAttribute("style", styleNormal + styleHeader);
                    row.appendChild(headerCell);

                    switch (reportName) {

                        case "51fb1a2e89534d957310":
                            headerCell = document.createElement("TH");
                            headerCell.setAttribute("style", styleNormal + styleHeader);
                            headerCell.innerHTML = tableArray[i][j].taxes;
                            row.appendChild(headerCell);

                            headerCell = document.createElement("TH");
                            headerCell.setAttribute("style", styleNormal + styleHeader);
                            headerCell.innerHTML = tableArray[i][j].total;
                            row.appendChild(headerCell);
                            break;


                        case "662140001cd759073b0d":
                            headerCell = document.createElement("TH");
                            headerCell.setAttribute("style", styleNormal + styleHeader);
                            headerCell.innerHTML = tableArray[i][j].taxes;
                            row.appendChild(headerCell);

                            headerCell = document.createElement("TH");
                            headerCell.setAttribute("style", styleNormal + styleHeader);
                            headerCell.innerHTML = tableArray[i][j].total;
                            row.appendChild(headerCell);
                            break;

                    }



                    headerCell = document.createElement("TH");
                    headerCell.setAttribute("style", styleNormal + styleHeader);
                    headerCell.innerHTML = tableArray[i][j].yoy;
                    row.appendChild(headerCell);

                }

            }
            else {


                var style = styleNormal;
                if (i == tableArray.length - 1 || j == tableArray[i].length - 1) {
                    style = style + styleHeader;
                }

                if (j > 0) {

                    style = style + 'text-align: center;';


                    var dataCell = document.createElement("TD");
                    dataCell.innerHTML = tableArray[i][j].cost;



                    dataCell.setAttribute("style", style);
                    row.appendChild(dataCell);

                    switch (reportName) {

                        case "51fb1a2e89534d957310":
                            dataCell = document.createElement("TD");
                            dataCell.innerHTML = tableArray[i][j].taxes;
                            dataCell.setAttribute("style", style);
                            row.appendChild(dataCell);

                            dataCell = document.createElement("TD");
                            dataCell.innerHTML = tableArray[i][j].total;
                            dataCell.setAttribute("style", style);
                            row.appendChild(dataCell);
                            break;

                        case "662140001cd759073b0d":
                            dataCell = document.createElement("TD");
                            dataCell.innerHTML = tableArray[i][j].taxes;
                            dataCell.setAttribute("style", style);
                            row.appendChild(dataCell);

                            dataCell = document.createElement("TD");
                            dataCell.innerHTML = tableArray[i][j].total;
                            dataCell.setAttribute("style", style);
                            row.appendChild(dataCell);
                            break;

                    }



                    dataCell = document.createElement("TD");
                    if (tableArray[i][j].yoy.indexOf('-') > -1) {
                        style = style + "color: red;"
                    }

                    dataCell.innerHTML = tableArray[i][j].yoy;
                    dataCell.setAttribute("style", style);
                    row.appendChild(dataCell);


                }
                else {

                    style = style + 'text-align: left;';

                    var dataCell = document.createElement("TD");
                    dataCell.innerHTML = tableArray[i][j].replace(/"/g, '');
                    dataCell.setAttribute("style", style);
                    row.appendChild(dataCell);
                }

            }
        }
    }

    $('#hdnHtmlCode').val($('#dvExportedData').html());
    document.getElementById('CreatePDFDocument').click();


}

function downloadCSV(csv, filename) {
    var csvFile;
    var downloadLink;

    // CSV file
    csvFile = new Blob([csv], { type: "text/csv" });

    // Download link
    downloadLink = document.createElement("a");

    // File name
    downloadLink.download = filename;

    // Create a link to the file
    downloadLink.href = window.URL.createObjectURL(csvFile);

    // Hide download link
    downloadLink.style.display = "none";

    // Add the link to DOM
    document.body.appendChild(downloadLink);

    // Click download link
    downloadLink.click();
}

function exportTableToCSV(filename) {
    var csv = [];
    var rows = document.querySelectorAll("#exportedData tr");

    for (var i = 0; i < rows.length; i++) {
        var row = [], cols = rows[i].querySelectorAll("td, th");

        for (var j = 0; j < cols.length; j++)
            row.push(cols[j].innerText);

        csv.push(row.join(","));
    }

    // Download CSV file
    downloadCSV(csv.join("\n"), filename);
}

function exportTableToCSVColsRows(columns, rows, filename) {
    var csv = [];
    csv.push(columns.join(','));
    for (var i = 0; i < rows.length; i++) {
        csv.push(rows[i].join(','));
    }
    // Download CSV file
    downloadCSV(csv.join("\n"), filename);
}

if (!library)
    var library = {};

library.json = {
    replacer: function (match, pIndent, pKey, pVal, pEnd) {
        var key = '<span class=json-key><b>';
        var val = '<span class=json-value><b>';
        var str = '<span class=json-string><b>';
        var r = pIndent || '';
        if (pKey)
            r = r + key + pKey.replace(/[": ]/g, '') + '</b></span>: ';
        if (pVal)
            r = r + (pVal[0] == '"' ? str : val) + pVal + '</span><br />';
        return r + (pEnd || '');
    },
    prettyPrint: function (obj) {
        var jsonLine = /^( *)("[\w]+": )?("[^"]*"|[\w.+-]*)?([,[{])?$/mg;
        return JSON.stringify(obj, null, 3)
            .replace(/&/g, '&amp;').replace(/\\"/g, '&quot;')
            .replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(jsonLine, library.json.replacer);
    }
};

String.prototype.splitComma = function () {
    return this.split(',').reduce((accum, curr) => {
        if (accum.isConcatting) {
            accum.soFar[accum.soFar.length - 1] += ',' + curr
        } else {
            accum.soFar.push(curr)
        }
        if (curr.split('"').length % 2 == 0) {
            accum.isConcatting = !accum.isConcatting
        }
        return accum;
    }, { soFar: [], isConcatting: false }).soFar
};