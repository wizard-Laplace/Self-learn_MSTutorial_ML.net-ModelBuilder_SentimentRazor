// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.


// OnGetAnalyzeSentiment ハンドラーへのユーザー入力を使用して GET HTTP要求を作成
function getSentiment(userInput) {
    return fetch(`Index?handler=AnalyzeSentiment&text=${userInput}`)
        .then((response) => {
            return response.text();
        })
}

// センチメントが予測されると、Webページ上のマーカーの位置を動的に更新
function updateMarker(markerPosition, sentiment) {
    $("#markerPosition").attr("style", `left:${markerPosition}%`);
    $("#markerValue").text(sentiment);
}

// ユーザーから入力を取得し、getSentiment 関数を使用してそれを OnGetAnalyzeSentiment 関数に送信し、
// updateMarker 関数でマーカーを更新
function updateSentiment() {

    var userInput = $("#Message").val();

    getSentiment(userInput)
        .then((sentiment) => {
            switch (sentiment) {
                case "Not Toxic":
                    updateMarker(100.0, sentiment);
                    break;
                case "Toxic":
                    updateMarker(0.0, sentiment);
                    break;
                default:
                    updateMarker(45.0, "Neutral");
            }
        });
}

// イベントハンドラーを登録し、id=Message 属性を使ってtextarea要素にバインド
$("#Message").on('change input paste', updateSentiment)