import {convertInvestmentModelData, flattenApiInvestment} from '../api/ApiMapper';
import {Investment, InvestmentModel} from '../Interfaces';

describe('flattenApiInvestment', () => {
	test('should flatten an investment object - stock version', () => {
		const apiData: Investment = {
			investmentId: 2,
			investmentName: 'Test Investment 2',
			investmentType: 'StockInvestment',
			investmentData: {
				stockPrice: '250',
				stockQuantity: '25',
				stockTicker: 'TZST',
				stockPurchaseDate: '1/25/2023 4:04:08 PM',
				stockDividendPercent: '0.05',
				stockDividendDistributionInterval: 'Month',
				stockDividendDistributionMethod: 'Stock',
				stockDividendFirstPaymentDate: '1/1/2020',
			},
			analysisOptionsOverrides: {
				analysisLength: '60',
				simCount: '1000',
				randomVariableMu: '0.05',
				randomVariableSigma: '0.1',
				randomVariableScaleFactor: '1',
			},
			analysisType: 'MonteCarlo_LogNormalDist',
			lastAnalysis: null,
			portfolioId: 1,
		};

		const expected = {
			investmentId: '2',
			investmentName: 'Test Investment 2',
			investmentType: 'StockInvestment',
			stockPrice: '250',
			stockQuantity: '25',
			stockTicker: 'TZST',
			stockPurchaseDate: '1/25/2023 4:04:08 PM',
			stockDividendPercent: '0.05',
			stockDividendDistributionInterval: 'Month',
			stockDividendDistributionMethod: 'Stock',
			stockDividendFirstPaymentDate: '1/1/2020',
			analysis_analysisLength: '60',
			analysis_simCount: '1000',
			analysis_randomVariableMu: '0.05',
			analysis_randomVariableSigma: '0.1',
			analysis_randomVariableScaleFactor: '1',
			analysisType: 'MonteCarlo_LogNormalDist',
		};

		const result = flattenApiInvestment(apiData);

		expect(result).toStrictEqual(expected);
	});
});

describe('convertInvestmentModelData', () => {
	test('converts to expected format', () => {
		const apiData: InvestmentModel = {
			investmentModelId: 3,
			investmentId: 1,
			maxModelData: [
				158.8545, 543.4615288093273, 866.6413607454344, 1043.3396795507128,
				1332.1016427834531, 1580.8647326680814, 1618.581953070835, 1637.069118966169,
				1951.2246626160368, 2362.6530275165132, 2606.242588289482, 2945.8751979815997,
				2859.718873568123, 3032.3249436168635, 3299.9168200112426, 3670.460731572893,
				3735.4999376021756, 4432.664997409087, 4346.6365075499125, 4715.947467903961,
				5134.689103247475, 5386.956422568682, 5806.831620025398, 6310.310292876733,
				6852.906518823868, 7737.410260765851, 8555.891139351503, 9308.564108305465,
				10337.635888739756, 11320.59879607934, 12142.974402311422, 12336.406361417461,
				12572.417191125753, 12159.065775386018, 13359.949706402134, 14243.30301828935,
				16756.01655014567, 18531.135004875177, 19750.734232671242, 20091.58551735901,
				22655.704881049238, 22370.578899550655, 23048.574898640294, 25154.31864927068,
				26374.023746250597, 27433.0476376027, 30631.12531670661, 32729.176254984566,
				33107.101770049994, 34966.25322231344, 35797.11652288241, 36677.61610120551,
				41302.678511119244, 41625.956866752225, 43086.22791964916, 46015.336819076445,
				47608.22543134721, 51306.46145417086, 55233.889868049686, 58053.45614002355,
			],
			minModelData: [
				158.8545, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0,
			],
			avgModelData: [
				158.8545, 161.87368034727155, 165.22412779628675, 172.3914187122669,
				176.52939209267228, 179.60281561743537, 189.71927207134243, 206.79584267329517,
				215.7540490396638, 228.42556755206093, 241.2144223946573, 252.2271525084375,
				272.0013158304313, 290.1026763577474, 301.50278664184987, 316.104111431159,
				324.63269669372715, 336.569497789251, 345.9531440949028, 347.507597154238,
				351.20032796370936, 362.6943881485147, 404.2192463285726, 418.34694358149324,
				448.5353905047537, 463.48800649368303, 471.1087194617444, 484.059814318672,
				520.6647408575901, 519.3801027275197, 524.9835422316412, 574.5685121948684,
				573.7102373378758, 635.0584808012958, 641.3531098431354, 666.3767706821247,
				727.1718081194709, 733.6373318093137, 757.0270273032328, 775.6262604602076,
				833.8820027448728, 929.6329497310267, 945.5235092354303, 999.655004814053,
				1034.8303522990034, 1060.9424866478973, 1093.5143894140228, 1123.7193792598623,
				1099.8430085407256, 1149.4256749862786, 1211.3210969793142, 1261.8005672136865,
				1262.432334039295, 1375.5156861220885, 1468.9294609950502, 1482.9774102725505,
				1488.5400497325024, 1541.7062539944895, 1638.4431934497932, 1728.9789904271543,
			],
			lastUpdated: '2023-02-13T17:22:07.4919216',
		};

		const result = convertInvestmentModelData(apiData);

		for (let i = 0; i < result.length; i++) {
			expect(result[i].year).toEqual(i);
			expect(result[i].min).toEqual(+apiData.minModelData[i].toFixed(2));
			expect(result[i].max).toEqual(+apiData.maxModelData[i].toFixed(2));
			expect(result[i].avg).toEqual(+apiData.avgModelData[i].toFixed(2));
		}
	});
});
