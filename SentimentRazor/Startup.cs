using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.Extensions.ML;
using SentimentRazorML.Model;

namespace SentimentRazor
{
    public class Startup
    {
        // トレーニング済みのモデルファイルの場所を格納するグローバル変数を定義
        private readonly string _modelPath;

        /// <summary>
        /// GetAbsolutePathメソッドを使用して、グローバル変数_modelPathに値を代入する
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _modelPath = GetAbsolutePath("MLModel.zip");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddPredictionEnginePool<ModelInput, ModelOutput>()
                .FromFile(_modelPath);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

        /// <summary>
        /// モデルファイルはアプリケーションのアセンブリファイルと共にビルドディレクトリに格納される為、
        /// アクセスしやすいようにヘルパーメソッドを実装する。
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns>fullPath</returns>
        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);
            return fullPath;
        }
    }
}
