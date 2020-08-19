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
        // �g���[�j���O�ς݂̃��f���t�@�C���̏ꏊ���i�[����O���[�o���ϐ����`
        private readonly string _modelPath;

        /// <summary>
        /// GetAbsolutePath���\�b�h���g�p���āA�O���[�o���ϐ�_modelPath�ɒl��������
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
        /// ���f���t�@�C���̓A�v���P�[�V�����̃A�Z���u���t�@�C���Ƌ��Ƀr���h�f�B���N�g���Ɋi�[�����ׁA
        /// �A�N�Z�X���₷���悤�Ƀw���p�[���\�b�h����������B
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
