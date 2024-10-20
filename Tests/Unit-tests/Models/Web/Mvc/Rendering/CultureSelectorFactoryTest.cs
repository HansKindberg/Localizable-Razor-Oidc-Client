using System.Globalization;
using Application.Models.Globalization;
using Application.Models.Web.Mvc.Rendering;
using Application.Models.Web.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;
using UnitTests.Helpers;

namespace UnitTests.Models.Web.Mvc.Rendering
{
	public class CultureSelectorFactoryTest
	{
		#region Methods

		[Fact]
		public async Task Create_Test_01()
		{
			await Task.CompletedTask;

			var instances = CreateInstances(null);
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[4];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("svenska", selectedListItem.Text);
			Assert.Equal("/", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_02()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/sv");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[4];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("svenska", selectedListItem.Text);
			Assert.Equal("/sv", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/sv", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_03()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/sv/");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[4];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("svenska", selectedListItem.Text);
			Assert.Equal("/sv/", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/sv/", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_04()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/en");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[1];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("English", selectedListItem.Text);
			Assert.Equal("/en", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_05()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/en/");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[1];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("English", selectedListItem.Text);
			Assert.Equal("/en/", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_06()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/sv-SE/en");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[1];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("English", selectedListItem.Text);
			Assert.Equal("/en", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_07()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/sv-SE/en/");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[1];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("English", selectedListItem.Text);
			Assert.Equal("/en/", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_08()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/first/second/third?a=b&C=D");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[4];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("svenska", selectedListItem.Text);
			Assert.Equal("/first/second/third?a=b&C=D", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/first/second/third?a=b&C=D", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/first/second/third?a=b&C=D", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/first/second/third?a=b&C=D", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/first/second/third?a=b&C=D", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/first/second/third?a=b&C=D", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_09()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/sv/first/second/third?a=b&C=D");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[4];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("svenska", selectedListItem.Text);
			Assert.Equal("/sv/first/second/third?a=b&C=D", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/first/second/third?a=b&C=D", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/first/second/third?a=b&C=D", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/first/second/third?a=b&C=D", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/first/second/third?a=b&C=D", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/sv/first/second/third?a=b&C=D", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_10()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/sv/first/second/third/?a=b&C=D");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[4];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("svenska", selectedListItem.Text);
			Assert.Equal("/sv/first/second/third/?a=b&C=D", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/first/second/third/?a=b&C=D", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/first/second/third/?a=b&C=D", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/first/second/third/?a=b&C=D", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/first/second/third/?a=b&C=D", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/sv/first/second/third/?a=b&C=D", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_11()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/en-001/sv/first/second/third?a=b&C=D");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[4];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("svenska", selectedListItem.Text);
			Assert.Equal("/sv/first/second/third?a=b&C=D", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/first/second/third?a=b&C=D", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/first/second/third?a=b&C=D", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/first/second/third?a=b&C=D", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/first/second/third?a=b&C=D", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/sv/first/second/third?a=b&C=D", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_12()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/en-001/sv/first/second/third/?a=b&C=D");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[4];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("svenska", selectedListItem.Text);
			Assert.Equal("/sv/first/second/third/?a=b&C=D", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/first/second/third/?a=b&C=D", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/first/second/third/?a=b&C=D", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/first/second/third/?a=b&C=D", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/first/second/third/?a=b&C=D", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/sv/first/second/third/?a=b&C=D", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_13()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/en/first/second/third?a=b&C=D");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[1];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("English", selectedListItem.Text);
			Assert.Equal("/en/first/second/third?a=b&C=D", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/first/second/third?a=b&C=D", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/first/second/third?a=b&C=D", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/first/second/third?a=b&C=D", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/first/second/third?a=b&C=D", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/first/second/third?a=b&C=D", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_14()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/en/first/second/third/?a=b&C=D");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[1];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("English", selectedListItem.Text);
			Assert.Equal("/en/first/second/third/?a=b&C=D", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/first/second/third/?a=b&C=D", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/first/second/third/?a=b&C=D", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/first/second/third/?a=b&C=D", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/first/second/third/?a=b&C=D", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/first/second/third/?a=b&C=D", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_15()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/sv-SE/en/first/second/third?a=b&C=D");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[1];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("English", selectedListItem.Text);
			Assert.Equal("/en/first/second/third?a=b&C=D", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/first/second/third?a=b&C=D", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/first/second/third?a=b&C=D", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/first/second/third?a=b&C=D", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/first/second/third?a=b&C=D", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/first/second/third?a=b&C=D", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_16()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/sv-SE/en/first/second/third/?a=b&C=D");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[1];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("English", selectedListItem.Text);
			Assert.Equal("/en/first/second/third/?a=b&C=D", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/first/second/third/?a=b&C=D", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/first/second/third/?a=b&C=D", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/first/second/third/?a=b&C=D", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/first/second/third/?a=b&C=D", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/first/second/third/?a=b&C=D", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_17()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/en-001");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[1];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("English", selectedListItem.Text);
			Assert.Equal("/en", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_18()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/en-001/");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[1];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("English", selectedListItem.Text);
			Assert.Equal("/en/", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_19()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/en-001/first/second/third?a=b&C=D");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[1];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("English", selectedListItem.Text);
			Assert.Equal("/en/first/second/third?a=b&C=D", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/first/second/third?a=b&C=D", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/first/second/third?a=b&C=D", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/first/second/third?a=b&C=D", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/first/second/third?a=b&C=D", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/first/second/third?a=b&C=D", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_20()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/en-001/first/second/third/?a=b&C=D");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[1];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("English", selectedListItem.Text);
			Assert.Equal("/en/first/second/third/?a=b&C=D", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/first/second/third/?a=b&C=D", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/first/second/third/?a=b&C=D", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/first/second/third/?a=b&C=D", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/first/second/third/?a=b&C=D", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/first/second/third/?a=b&C=D", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_21()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/en-001/fi/first/second/third?a=b&C=D");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[3];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("suomi", selectedListItem.Text);
			Assert.Equal("/fi/first/second/third?a=b&C=D", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/first/second/third?a=b&C=D", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/first/second/third?a=b&C=D", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/first/second/third?a=b&C=D", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/first/second/third?a=b&C=D", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/first/second/third?a=b&C=D", cultureSelector.List[4].Value);
		}

		[Fact]
		public async Task Create_Test_22()
		{
			await Task.CompletedTask;

			var instances = CreateInstances("/en-001/fi/first/second/third/?a=b&C=D");
			var cultureSelector = instances.Item1.Create(instances.Item2);

			Assert.NotNull(cultureSelector);
			Assert.Equal(5, cultureSelector.List.Count);

			var selectedListItem = cultureSelector.List[3];
			Assert.True(selectedListItem.Selected);
			Assert.Equal("suomi", selectedListItem.Text);
			Assert.Equal("/fi/first/second/third/?a=b&C=D", selectedListItem.Value);

			Assert.Equal("Deutsch", cultureSelector.List[0].Text);
			Assert.Equal("/de/first/second/third/?a=b&C=D", cultureSelector.List[0].Value);

			Assert.Equal("English", cultureSelector.List[1].Text);
			Assert.Equal("/en/first/second/third/?a=b&C=D", cultureSelector.List[1].Value);

			Assert.Equal("français", cultureSelector.List[2].Text);
			Assert.Equal("/fr/first/second/third/?a=b&C=D", cultureSelector.List[2].Value);

			Assert.Equal("suomi", cultureSelector.List[3].Text);
			Assert.Equal("/fi/first/second/third/?a=b&C=D", cultureSelector.List[3].Value);

			Assert.Equal("svenska", cultureSelector.List[4].Text);
			Assert.Equal("/first/second/third/?a=b&C=D", cultureSelector.List[4].Value);
		}

		private static ICultureContext CreateCultureContext(CultureInfo currentCulture, CultureInfo currentUiCulture)
		{
			var cultureContextMock = new Mock<ICultureContext>();

			cultureContextMock.Setup(cultureContext => cultureContext.CurrentCulture).Returns(currentCulture);
			cultureContextMock.Setup(cultureContext => cultureContext.CurrentUiCulture).Returns(currentUiCulture);
			cultureContextMock.Setup(cultureContext => cultureContext.MasterCulture).Returns(CultureInfo.GetCultureInfo(LocalizationHelper.MasterCulture));
			cultureContextMock.Setup(cultureContext => cultureContext.MasterUiCulture).Returns(CultureInfo.GetCultureInfo(LocalizationHelper.MasterUiCulture));

			return cultureContextMock.Object;
		}

		private static CultureSelectorFactory CreateCultureSelectorFactory(CultureInfo currentCulture, CultureInfo currentUiCulture)
		{
			var cultureContext = CreateCultureContext(currentCulture, currentUiCulture);
			var requestLocalizationOptions = CreateRequestLocalizationOptions();
			var requestLocalizationOptionsMonitor = CreateRequestLocalizationOptionsMonitor(requestLocalizationOptions);

			return new CultureSelectorFactory(cultureContext, requestLocalizationOptionsMonitor);
		}

		private static Tuple<CultureSelectorFactory, UrlHelperBase> CreateInstances(string? pathAndQuery)
		{
			WebHelper.ResolvePathAndQuery(pathAndQuery, out var cultureRoute, out var currentCulture, out var currentUiCulture, out var path, out var query, out var uiCultureRoute);

			var cultureSelectorFactory = CreateCultureSelectorFactory(currentCulture, currentUiCulture);
			var urlHelper = CreateUrlHelper(cultureRoute, path, query, uiCultureRoute);

			return new Tuple<CultureSelectorFactory, UrlHelperBase>(cultureSelectorFactory, urlHelper);
		}

		private static RequestLocalizationOptions CreateRequestLocalizationOptions()
		{
			var options = new RequestLocalizationOptions();

			options.SupportedCultures!.Clear();
			foreach(var culture in LocalizationHelper.Cultures)
			{
				options.SupportedCultures.Add(CultureInfo.GetCultureInfo(culture));
			}

			options.SupportedUICultures!.Clear();
			foreach(var uiCulture in LocalizationHelper.UiCultures)
			{
				options.SupportedUICultures.Add(CultureInfo.GetCultureInfo(uiCulture));
			}

			return options;
		}

		private static IOptionsMonitor<RequestLocalizationOptions> CreateRequestLocalizationOptionsMonitor(RequestLocalizationOptions options)
		{
			var localizationOptionsMonitorMock = new Mock<IOptionsMonitor<RequestLocalizationOptions>>();

			localizationOptionsMonitorMock.Setup(localizationOptionsMonitor => localizationOptionsMonitor.CurrentValue).Returns(options);

			return localizationOptionsMonitorMock.Object;
		}

		/// <summary>
		/// https://stackoverflow.com/questions/55922148/unit-testing-urlhelper-extensions#answer-55922889
		/// </summary>
		private static UrlHelperBase CreateUrlHelper(string? cultureRoute, string? path, string? query, string? uiCultureRoute)
		{
			var actionContext = new ActionContext
			{
				ActionDescriptor = new ActionDescriptor(),
				HttpContext = new DefaultHttpContext(),
				RouteData = new RouteData()
			};

			actionContext.HttpContext.Request.Host = new HostString("localhost");
			actionContext.HttpContext.Request.Path = path;
			actionContext.HttpContext.Request.QueryString = new QueryString(query);
			actionContext.HttpContext.Request.Scheme = "https";

			if(cultureRoute != null)
				actionContext.RouteData.Values.Add(RouteKeys.Culture, cultureRoute);

			if(uiCultureRoute != null)
				actionContext.RouteData.Values.Add(RouteKeys.UiCulture, uiCultureRoute);

			var urlHelperMock = new Mock<UrlHelperBase>(actionContext) { CallBase = true };

			return urlHelperMock.Object;
		}

		#endregion
	}
}