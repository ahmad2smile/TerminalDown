$(document).ready(function () {

    $(".button-collapse").sideNav();
    $(".dropdown-button").dropdown({
        belowOrigin: true,
        alignment: 'right'
    });


    function duoGraph() {

        if (document.getElementById('DuoFirstVotes') && document.getElementById('DuoSecondVotes')) {

            let duoFirstVotes = document.getElementById('DuoFirstVotes').innerHTML;
            let duoSecondVotes = document.getElementById('DuoSecondVotes').innerHTML;

            let data = [{ xData: "DuoFirst", yData: duoFirstVotes }, { xData: "DuoSecond", yData: duoSecondVotes }];

            let width = 150,
                height = 300;

            let x = d3.scaleBand()
                      .range([0, width])
                      .padding(0.1);
            let y = d3.scaleLinear()
                      .range([height, 0]);

            let svg = d3.select(".chart").append("svg")
                .attr("width", width)
                .attr("height", height)
                .append("g");

            x.domain(data.map(function (d) { return d.xData; }));
            y.domain([0, d3.max(data, function (d) { return d.yData; })]);

            svg.selectAll(".bar")
                .data(data)
                .enter().append("rect")
                .attr("class", "bar")
                .attr("x", function (d) { return x(d.xData); })
                .attr("width", x.bandwidth())
                .attr("y", function (d) { return y(d.yData); })
                .attr("height", function (d) { return height - y(d.yData); });
        }
    }

    duoGraph();

    function xhrVoteSubmit(submitedForm) {
        //    let xhrReq = new XMLHttpRequest();
        //    let duoFormsCon = document.getElementById("DuoFormsCon");
        //    let firstFormId = document.getElementById("FirstFormId");
        //    let secondFormId = document.getElementById("SecondFormId");
        //
        //    xhrReq.open("POST", submitedForm.action, true);
        //    xhrReq.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        //
        //    let formData = new FormData(submitedForm);
        //
        //    xhrReq.send(formData);
        //    console.log(formData);
        //    xhrReq.onreadystatechange = function() {
        //        duoFormsCon.innerHTML = xhrReq.responseText;
        //    };
        return false;
    }

    $('body').on("click", ".formClass", function (e) {

        e.preventDefault();
        let id = $(this).children("[name = 'Id']")[0].value;
        let votedName = $(this).children("[name = 'VotedName']")[0].value;

        $.post("/Home/Duos",
            { id: id, votedName: votedName },
            function (data) {
                $("#DuoFormsCon").html(data);
                duoGraph();
            });

    });

    $('#logOutLink').click(
        function() {
            $("#logOutForm").submit();
        });

    $('#mobile-demo li:last-child').click(
        function () {
            $("#logOutForm").submit();
        });

});