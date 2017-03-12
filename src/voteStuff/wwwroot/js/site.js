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

            let data = [{ xData: "DuoFirst", yData: parseInt(duoFirstVotes,10) }, { xData: "DuoSecond", yData: parseInt(duoSecondVotes,10) }];

            let width = 150,
                height = 300;

            let x = d3.scaleBand()
                      .range([0, width])
                      .padding(0.1);
            let y = d3.scaleLinear()
                      .range([height, 0]);
            $(".chart").children().remove();
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


    $('#logOutLink').click(
        function () {
            $("#logOutForm").submit();
        });

    $('#mobile-demo li:last-child').click(
        function () {
            $("#logOutForm").submit();
        });


    function updateDuo(data) {
        if (data) {
            $("#DuoFormsCon").html(data);
            console.log(data);
        }
        duoGraph();
    }

    function profileComponentUpdate(data) {
        $(".profile-component").parent().html(data);
    }


    $('body').on("click", ".formClass", function (e) {
        e.preventDefault();
        let postData = {
            id: $(this).children("[name = 'Id']")[0].value,
            votedName: $(this).children("[name = 'VotedName']")[0].value
        };
        $.ajax({
            method: "POST",
            url: "/Home/Duos",
            data: postData
        })
        .done(function (model) {
            updateDuo(model);
            $.ajax({
                method: "GET",
                url: "/Home/ProfileComponentUpdate",
                dataType: "HTML"
            })
        .done(function (data) {
            profileComponentUpdate(data);
        });
        });
    });

    //    signalR stuff

    function SignalR_updateDuo(SignalR_data) {
        $("#DuoTotallVotes").html(SignalR_data.duoTotalVotes);
        $("#DuoFirstVotes").html(SignalR_data.duoFirstVotes);
        $("#DuoSecondVotes").html(SignalR_data.duoSecondVotes);
        duoGraph();
    }

    $.connection.hub.logging = false;
    $.connection.hub.start();
    $.connection.votingHub.client.updateVotedDuo = SignalR_updateDuo;

    //    signalR Stuff End



    //    tooltip enableling

    $('.tooltipped').tooltip({ delay: 50 });

    //    tooltip enabling end

});
