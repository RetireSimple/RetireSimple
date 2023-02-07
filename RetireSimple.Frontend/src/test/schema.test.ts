import {investmentFormSchema} from '../data/FormSchema';

describe('schema validation', () => {
	describe('investmentType: StockInvestment', () => {
		test('MonteCarlo_NormalDist_Valid', () => {
			const schema = investmentFormSchema;
			const data = {
				investmentName: 'Test',
				investmentType: 'StockInvestment',
				stockTicker: 'TEST',
				stockPrice: '123.45',
				stockQuantity: '123.45',
				stockPurchaseDate: '2023-01-30',
				stockDividendPercent: '0.05',
				stockDividendDistributionInterval: 'Month',
				stockDividendDistributionMethod: 'Stock',
				stockDividendFirstPaymentDate: '2023-01-30',
				analysisType: 'MonteCarlo_NormalDist',
				analysisLength: '60',
				simCount: '10000',
				randomVariableMu: '1.23',
				randomVariableSigma: '1.23',
				randomVariableScaleFactor: '1.23',
			};

			expect(schema.isValidSync(data)).toBeTruthy();
		});

		test('MonteCarlo_NormalDist_Invalid_NoUndefined', () => {
			const schema = investmentFormSchema;
			const data = {
				investmentName: '',
				investmentType: 'StockInvestment',
				stockTicker: undefined,
				stockPrice: undefined,
				stockQuantity: undefined,
				stockPurchaseDate: undefined,
				stockDividendPercent: undefined,
				stockDividendDistributionInterval: undefined,
				stockDividendDistributionMethod: undefined,
				stockDividendFirstPaymentDate: undefined,
				analysisType: 'MonteCarlo_NormalDist',
				analysisLength: undefined,
				simCount: undefined,
				randomVariableMu: undefined,
				randomVariableSigma: undefined,
				randomVariableScaleFactor: undefined,
			};

			expect(schema.isValidSync(data)).toBeFalsy();
		});

		test('MonteCarlo_LogNormalDist_Valid', () => {
			const schema = investmentFormSchema;
			const data = {
				investmentName: 'Test',
				investmentType: 'StockInvestment',
				stockTicker: 'TEST',
				stockPrice: '123.45',
				stockQuantity: '123.45',
				stockPurchaseDate: '2023-01-30',
				stockDividendPercent: '0.05',
				stockDividendDistributionInterval: 'Month',
				stockDividendDistributionMethod: 'Stock',
				stockDividendFirstPaymentDate: '2023-01-30',
				analysisType: 'MonteCarlo_LogNormalDist',
				analysisLength: '60',
				simCount: '10000',
				randomVariableMu: '1.23',
				randomVariableSigma: '1.23',
				randomVariableScaleFactor: '1.23',
			};

			expect(schema.isValidSync(data)).toBeTruthy();
		});

		test('testAnalysis_Valid', () => {
			const schema = investmentFormSchema;
			const data = {
				investmentName: 'Test',
				investmentType: 'StockInvestment',
				stockTicker: 'TEST',
				stockPrice: '123.45',
				stockQuantity: '123.45',
				stockPurchaseDate: '2023-01-30',
				stockDividendPercent: '0.05',
				stockDividendDistributionInterval: 'Month',
				stockDividendDistributionMethod: 'Stock',
				stockDividendFirstPaymentDate: '2023-01-30',
				analysisType: 'testAnalysis',
			};

			expect(schema.isValidSync(data)).toBeTruthy();
		});
	});
});
