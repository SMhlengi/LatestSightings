<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Graphs.aspx.cs" Inherits="LatestSightings.Graphs" %>

jQuery(document).ready(function () {
    var m1 = new Morris.Line({
        // ID of the element in which to draw the chart.
        element: 'line-chart',
        // Chart data records -- each entry in this array corresponds to a point on
        // the chart.
        data: [
            <%=data%>
        ],
        xkey: 'y',
        ykeys: ['<%=ykeyA %>', '<%=ykeyB %>'],
        labels: ['<%=ykeyALabel %>', '<%=ykeyBLabel %>'],
        lineColors: ['#428BCA', '#a7cfef'],
        lineWidth: '2px',
        hideHover: 'auto',
        resize: true,
        //parseTime: false,
        preUnits: '$',
        xLabels: 'day'
    });

    var m2 = new Morris.Line({
        // ID of the element in which to draw the chart.
        element: 'line-chart1',
        // Chart data records -- each entry in this array corresponds to a point on
        // the chart.
        data: [
            <%=data1%>
        ],
        xkey: 'y',
        ykeys: ['<%=ykeyA %>', '<%=ykeyB %>'],
        labels: ['<%=ykeyALabel %>', '<%=ykeyBLabel %>'],
        lineColors: ['#428BCA', '#a7cfef'],
        lineWidth: '2px',
        hideHover: 'auto',
        resize: true,
        //parseTime: false,
        xLabels: 'day'
    });

    var m3 = new Morris.Line({
        // ID of the element in which to draw the chart.
        element: 'line-chart2',
        // Chart data records -- each entry in this array corresponds to a point on
        // the chart.
        data: [
            <%=data2%>
        ],
        xkey: 'y',
        ykeys: [<%=m3Keys %>],
        labels: [<%=m3Labels %>],
        lineColors: ['#428BCA', '#b53030', '#963c8a', '#5ba328', '#6f726d'],
        lineWidth: '2px',
        hideHover: 'auto',
        resize: true,
        //parseTime: false,
        preUnits: '$',
        xLabels: 'day'
    });

    var m4 = new Morris.Line({
        // ID of the element in which to draw the chart.
        element: 'line-chart3',
        // Chart data records -- each entry in this array corresponds to a point on
        // the chart.
        data: [
            <%=data3%>
        ],
        xkey: 'y',
        ykeys: [<%=m4Keys %>],
        labels: [<%=m4Labels %>],
        lineColors: ['#428BCA', '#b53030', '#963c8a', '#5ba328', '#6f726d'],
        lineWidth: '2px',
        hideHover: 'auto',
        resize: true,
        //parseTime: false,
        xLabels: 'day'
    });
});