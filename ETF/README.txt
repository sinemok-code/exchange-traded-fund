MELIH OZTURK

Starting Application
Solution configured to run Web API and MVC project together. Browser will automatically redirect to Upload File page.

Using Application
Please ensure that stock file upladed via Upload File page on MVC project. Then please check out the dashboards by selecting "ETF Dashboard" from top menu.
Also you can filter chart data by selecting start and end date from controls on the top of the page and by clicking Update.

Dashboard contains 3 charts
1- ETF Index Level
2- Stock Weighted Indices Data
3- Top 5 weighted stocks for the latest date

Decisions on Application
	Application Architecture
		- To ensure high quality for TDD, all component dependencies injected via Castle Windsor.
	Business
		- All calculation results rounded by 3 digits to increase readability.