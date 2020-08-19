using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ML;
using SentimentRazorML.Model;

namespace SentimentRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        // IndexModel クラス内で PredictionEnginePool を参照する変数を追加
        private readonly PredictionEnginePool<ModelInput, ModelOutput> _predictionEnginePool;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}

        /// <summary>
        /// IndexModel クラスでコンストラクターを作成し、PredictionEnginePool サービスを挿入
        /// </summary>
        /// <param name="predictionEnginePool"></param>
        public IndexModel(PredictionEnginePool<ModelInput, ModelOutput> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }

        public void OnGet()
        {

        }

        // OnGetAnalyzeSentiment メソッドを新規追加
        public IActionResult OnGetAnalyzeSentiment([FromQuery] string text)
        {
            // ユーザーからの入力が空白またはnullの場合、OnGetAnalyzeSentiment メソッド内で、
            // "ニュートラル"センチメントを返す。
            if (String.IsNullOrEmpty(text))
                return Content("Neutral");

            // 入力が有効な場合は、ModelInputの新しいインスタンスを作成
            var input = new ModelInput { SentimentText = text };

            // PredictionEnginePoolを使用してセンチメントを予測する
            var prediction = _predictionEnginePool.Predict(input);

            // 予測されたbool値を有害または無害に変換します
            var sentiment = Convert.ToBoolean(prediction.Prediction) ? "Toxic" : "Not Toxic";

            // センチメントをWebページに返す
            return Content(sentiment);
        }
    }
}
