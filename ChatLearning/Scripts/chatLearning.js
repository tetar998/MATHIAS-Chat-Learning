function DisplayInitialization() {
    $(".panel-body").height($(window).height() - 215);
}


function UserMessage(sentence) {
    return '<li class="clearfix">' +
                '<div class="chat-body clearfix chat-green">' +
                    '<div class="header">' +
                        '<strong class="primary-font">Utilisateur</strong>' +
                    '</div>' +
                    '<p>' + sentence + '</p>' +
                '</div>' +
            '</li>';
}

function ComputerMessage(sentence) {
    return '<li class="clearfix computer">' +
                '<div class="chat-body clearfix chat-red">' +
                    '<div class="header">' +
                        '<strong class="primary-font">Ordinateur</strong>' +
                    '</div>' +
                    '<p>' + sentence + '</p>' +
                '</div>' +
            '</li>';
}

var ComputerMessage2 = '<li class="clearfix">' +
                            '<div class="chat-body clearfix chat-red">' +
                                '<div class="header">' +
                                    '<strong class="primary-font">Ordinateur</strong>' +
                                '</div>' +
                                '<p>Je n\'ai pas de réponse... Que voulez-vous que je réponde la prochaine fois ?</p>' +
                                '<div class="input-group">' +
                                    '<input id="answer-contains" type="text" class="form-control input-sm" placeholder="Ecrivez la réponse attendus ici..." />' +
                                    '<span class="input-group-btn">' +
                                        '<button class="btn btn-sm" id="btn-answer">Enregistrer</button>' +
                                    '</span>' +
                                '</div>' +
                            '</div>' +
                        '</li>';

var ComputerLoading = '<li id="loading" class="clearfix">' +
                            '<div class="chat-body clearfix chat-loading">' +
                                '<div class="header">' +
                                    '<strong class="primary-font">Ordinateur</strong>' +
                                '</div>' +
                                '<div>' +
                                    '<span class="glyphicon glyphicon-refresh glyphicon-refresh-animate"></span>Chargement... ' +
                                '</div>' +
                            '</div>' +
                        '</li>';

function ComputerLearnSentence(sentence) {

    $(".chat").append(ComputerLoading);

    $.ajax({
        type: "POST",
        url: "/Home/AddAnswer",
        data: "{'sentence': '" + sentence.replace(/'/g, "\\'") + "', 'answer': '" + $("#answer-contains").val().replace(/'/g, "\\'") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#loading").remove();
            $("#answer-contains").parent().remove();
            $(".chat").append(ComputerMessage(data));
        }
    })
}

function SendMessage(sentence) {

    if ($("#answer-contains").length) {
        $("#answer-contains").parent().remove();
    }

    $(".chat").append(UserMessage(sentence));

    $("#sentence-contains").val("");

    $(".chat").append(ComputerLoading);

    $.ajax({
        type: "POST",
        url: "/Home/GetAnswer",
        data: "{'sentence': '" + sentence.replace(/'/g, "\\'") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#loading").remove();
            $(".chat").append(ComputerMessage(data));
        },
        error: function () {
            $("#loading").remove();
            $(".chat").append(ComputerMessage2);

            $("#answer-contains").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: "/Home/GetAnswersTags",
                        data: "{'text': '" + $("#answer-contains").val().replace(/'/g, "\\'") + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            response(data);
                        }
                    });
                }
            });

            $("#btn-answer").click(function () {
                ComputerLearnSentence(sentence);
            });

            $("#answer-contains").on('keydown', function (e) {
                if (e.which == 13 && $(this).val() != "") {
                    ComputerLearnSentence(sentence);
                }
            });
        }
    });

    $('.panel-body').animate({ scrollTop: $(document).height() });
}

$(document).ready(function () {

    DisplayInitialization();

	$("#sentence-contains").on('keydown', function(e) {
	    if (e.which == 13 && $(this).val() != "") {
	        SendMessage($(this).val());
	    }
	});

	$("#btn-sentence").click(function() {
	    if ($("#sentence-contains").val() != "") {
	        SendMessage($("#btn-input").val());
	    }
	});

	$("#refresh").click(function () {
	    $(".chat").html("");
	})
})

$(window).resize(function () {
    DisplayInitialization();
})