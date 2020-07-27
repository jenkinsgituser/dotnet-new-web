﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TemplatesShared;
using Xunit;

namespace TemplateTest {
    public class NuGetHelperTests {
        private HttpClient httpClient = new HttpClient();
        [Fact]
        public async Task TestGetAllQueryUrisAsync01Async() {
            var nugetApiHelper = new NuGetHelper(new RemoteFile());

            var results = await nugetApiHelper.GetAllQueryUrisAsync(httpClient, "template", 20);
            Assert.NotEmpty(results);
        }

        [Fact]
        public async Task TestQueryNuGetAsync01Async() {
            var nugetApiHelper = new NuGetHelper(new RemoteFile());

            var results = await nugetApiHelper.QueryNuGetAsync(httpClient, "dec");
            Assert.True(results.Count > 300);
        }
    }
    public class NuGetPackageDownloaderTests {
        private HttpClient httpClient;
        private IRemoteFile remoteFile;
        private INuGetHelper nugetHelper;
        private INuGetPackageDownloader nugetDownloader;
        public NuGetPackageDownloaderTests() {
            httpClient = new HttpClient();
            remoteFile = new RemoteFile();
            nugetHelper = new NuGetHelper(remoteFile);
            nugetDownloader = new NuGetPackageDownloader(nugetHelper, remoteFile);
        }

        [Fact]
        public async Task TestDownloadAllPackagesAsync01Async() {
            var packages = await nugetHelper.QueryNuGetAsync(httpClient, "dec");
            var downloader = new NuGetPackageDownloader(nugetHelper, remoteFile);

            var result = await downloader.DownloadAllPackagesAsync(packages);

            Assert.NotNull(result);
            Assert.Equal(packages.Count, result.Count);
            Assert.True(!string.IsNullOrEmpty(packages[0].LocalFilepath));
            Assert.True(File.Exists(packages[0].LocalFilepath));
        }

        [Fact]
        public async Task TestTemplateReport01Async() {
            var templateReport = new TemplateReport(nugetHelper, httpClient, nugetDownloader, remoteFile);
            await templateReport.GenerateTemplateJsonReportAsync(new string[] { "template" }, @"");

            Assert.True(1 == 1);
        }
    }
}
