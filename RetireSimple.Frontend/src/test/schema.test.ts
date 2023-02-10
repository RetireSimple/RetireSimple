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
				analysis_analysisLength: '60',
				analysis_simCount: '10000',
				analysis_randomVariableMu: '1.23',
				analysis_randomVariableSigma: '1.23',
				analysis_randomVariableScaleFactor: '1.23',
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
				analysis_analysisLength: undefined,
				analysis_simCount: undefined,
				analysis_randomVariableMu: undefined,
				analysis_randomVariableSigma: undefined,
				analysis_randomVariableScaleFactor: undefined,
			};

			expect(schema.isValidSync(data)).toBeFalsy();
		});

		test('MonteCarlo_NormalDist_Invalid_NaN_Inputs', () => {
			const schema = investmentFormSchema;
			const data = {
				investmentName: 'Test',
				investmentType: 'StockInvestment',
				stockTicker: 'TEST',
				stockPrice: '',
				stockQuantity: '',
				stockPurchaseDate: '2023-01-30',
				stockDividendPercent: '',
				stockDividendDistributionInterval: 'Month',
				stockDividendDistributionMethod: 'Stock',
				stockDividendFirstPaymentDate: '2023-01-30',
				analysisType: 'MonteCarlo_NormalDist',
				analysis_analysisLength: '',
				analysis_simCount: '',
				analysis_randomVariableMu: '',
				analysis_randomVariableSigma: '',
				analysis_randomVariableScaleFactor: '',
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
				analysis_analysisLength: '60',
				analysis_simCount: '10000',
				analysis_randomVariableMu: '1.23',
				analysis_randomVariableSigma: '1.23',
				analysis_randomVariableScaleFactor: '1.23',
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

	describe('investmentType: BondInvestment', () => {
		describe('General Bond Validation', () => {
			test('standardBond_Valid', () => {
				const schema = investmentFormSchema;
				const data = {
					investmentName: 'Test',
					investmentType: 'BondInvestment',
					bondTicker: 'TEST',
					bondYieldToMaturity: '0.05',
					bondPurchasePrice: '123.45',
					bondPurchaseDate: '2023-01-30',
					bondCurrentPrice: '123.45',
					bondCouponRate: '0.05',
					bondMaturityDate: '2023-01-30',
					analysisType: 'example',
					analysis_analysisLength: '60',
				};

				expect(schema.isValidSync(data)).toBeTruthy();
			});
		});
	});
});
