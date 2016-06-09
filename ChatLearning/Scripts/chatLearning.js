//  Permet de gérer la hauteur du Chat en fonction de la taille de l'écran
function DisplayInitialization() {
    $(".panel-body").height($(window).height() - 215);
}

//  Message de l'utilisateur
function UserMessage(sentence) {
    return '<li class="clearfix user">' +
                '<div class="chat-body clearfix chat-green">' +
                    '<div class="header">' +
                        '<strong class="primary-font">Utilisateur</strong>' +
                    '</div>' +
                    '<p>' + sentence + '</p>' +
                '</div>' +
                '<span class="fa fa-cog" data-toggle="modal" data-target="#ConfigurationPlugin"></span>' +
            '</li>';
}

//  Réponse de l'ordinateur
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

//  Réponse de l'ordinateur dans le cas ou il ne connais pas la phrase de l'utilisateur
var ComputerMessage2 = '<li class="clearfix computer">' +
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

//  Chargement de la réponse de l'ordinateur
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

//  Apprentissage d'une réponse par l'ordinateur
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

//  Permet d'écrir une phrase dans le Chat
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

    //  Envoi la phrase écrite sur le Chat après avoir appuyer sur ENTREE
	$("#sentence-contains").on('keydown', function(e) {
	    if (e.which == 13 && $(this).val() != "") {
	        SendMessage($(this).val());
	    }
	});

    //  Envoi de la phrase écrite sur le Chat après clique sur le bouton
	$("#btn-sentence").click(function() {
	    if ($("#sentence-contains").val() != "") {
	        SendMessage($("#btn-input").val());
	    }
	});

    //  Permet de raffraichir le Chat
	$("#refresh").click(function () {
	    $(".chat").html("");
	})

	$("#ConfigurationPlugin").on('shown.bs.modal', function () {
	    $("#ConfigurationPlugin .modal-body").html("");

	    $.ajax({
	        type: "POST",
	        url: "/Home/GetListOfPlugins",
	        data: "",
	        contentType: "application/json; charset=utf-8",
	        dataType: "json",
	        success: function (data) {
	            $.each(data, function (index, value) {
	                $("#ConfigurationPlugin .modal-body").append('<label><input type="checkbox" name="checkbox" value="' + index + '">' + value + '</label>')
	            })
	        }
	    });
	})
})

$(window).resize(function () {
    DisplayInitialization();
})