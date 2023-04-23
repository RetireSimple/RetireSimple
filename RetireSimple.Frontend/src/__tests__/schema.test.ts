import {Schema} from 'yup';
import {decimalValidation, investmentFormSchema, vehicleFormSchema} from '../forms/FormSchema';

describe('schema validation', () => {
	describe('decimal validation', () => {
		test('normal validation 1', () => {
			expect(decimalValidation(2, 1)).toBeTruthy();
			expect(decimalValidation(2, 1.1)).toBeTruthy();
			expect(decimalValidation(3, 1.123)).toBeTruthy();
			expect(decimalValidation(4, 1.1234)).toBeTruthy();
			expect(decimalValidation(5, 1.12345)).toBeTruthy();
			expect(decimalValidation(5, 1.123456)).toBeFalsy();
		});

		test('validation with zero', () => {
			expect(decimalValidation(2, 0)).toBeTruthy();
			expect(decimalValidation(2, 0.0)).toBeTruthy();
			expect(decimalValidation(2, 0.0)).toBeTruthy();
		});
	});

	describe('investmentType: StockInvestment', () => {
		test('MonteCarlo_DefinedPreset_Valid', () => {
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
				analysisType: 'MonteCarlo',
				analysis_analysisLength: '60',
				analysis_analysisPreset: 'DefaultStockAnalysis', //NOTE validation for the actual value is kinda pointless, but it's here for completeness
			};

			expect(schema.isValidSync(data)).toBeTruthy();
		});

		test('MonteCarlo_CustomPreset_Valid', () => {
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
				analysisType: 'MonteCarlo',
				analysis_analysisPreset: 'Custom',
				analysis_analysisLength: '60',
				analysis_randomVariableType: 'Normal',
				analysis_simCount: '10000',
				analysis_randomVariableMu: '1.23',
				analysis_randomVariableSigma: '1.23',
				analysis_randomVariableScaleFactor: '1.23',
			};

			expect(schema.isValidSync(data)).toBeTruthy();
		});

		test('MonteCarlo_CustomPreset_Invalid_NoUndefined', () => {
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
				analysisType: 'MonteCarlo',
				analysis_analysisPreset: 'Custom',
				analysis_analysisLength: undefined,
				analysis_simCount: undefined,
				analysis_randomVariableType: undefined,
				analysis_randomVariableMu: undefined,
				analysis_randomVariableSigma: undefined,
				analysis_randomVariableScaleFactor: undefined,
			};

			expect(schema.isValidSync(data)).toBeFalsy();
		});

		test('MonteCarlo_CustomPreset_Invalid_NaN_Inputs', () => {
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
				analysisType: 'MonteCarlo',
				analysis_analysisPreset: 'Custom',
				analysis_analysisLength: '',
				analysis_randomVariableType: 'Normal',
				analysis_simCount: '',
				analysis_randomVariableMu: '',
				analysis_randomVariableSigma: '',
				analysis_randomVariableScaleFactor: '',
			};

			expect(schema.isValidSync(data)).toBeFalsy();
		});

		test('#176 - Short Tickers', () => {
			const subschema = investmentFormSchema.fields.stockTicker as Schema;
			expect(subschema.isValidSync('A')).toBeTruthy();
			expect(subschema.isValidSync('AA')).toBeTruthy();
		});

		test('#184 - Zero Percent Dividend', () => {
			const subschema = investmentFormSchema.fields.stockDividendPercent as Schema;
			expect(subschema.isValidSync(0)).toBeTruthy();
			expect(subschema.isValidSync(0.0)).toBeTruthy();
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
					bondFaceValue: '123.45',
					bondPurchaseDate: '2023-01-30',
					bondCurrentPrice: '123.45',
					bondCouponRate: '0.05',
					bondMaturityDate: '2023-01-30',
					analysisType: 'StdBondValuation',
					analysis_analysisLength: '60',
					analysis_isAnnual: 'true',
				};

				expect(schema.isValidSync(data)).toBeTruthy();
			});
		});

		test('#176 - Short Tickers', () => {
			const subschema = investmentFormSchema.fields.bondTicker as Schema;
			expect(subschema.isValidSync('A')).toBeTruthy();
			expect(subschema.isValidSync('AA')).toBeTruthy();
		});
	});

	describe('investmentType: PensionInvestment', () => {
		describe('General Pension Validation', () => {
			test('standardPension_Valid', () => {
				const schema = investmentFormSchema;
				const data = {
					investmentName: 'Test',
					investmentType: 'PensionInvestment',
					pensionStartDate: '2023-01-30',
					pensionInitialMonthlyPayment: '123.45',
					pensionYearlyIncrease: '0.05',
					analysisType: 'PensionSimulation',
					analysis_analysisLength: '60',
					analysis_expectedTaxRate: '0.0',
				};

				expect(schema.isValidSync(data)).toBeTruthy();
			});
		});
	});

	describe('investmentVehicles', () => {
		describe('General 401k Validation', () => {
			test('standard401k_Valid: fixed contribution', () => {
				const schema = vehicleFormSchema;
				const data = {
					investmentVehicleName: 'Test',
					investmentVehicleType: 'Vehicle401k',
					cashHoldings: '0',
					analysis_analysisLength: '60',
					analysis_shortTermCapitalGainsTax: '0.0',
					analysis_longTermCapitalGainsTax: '0.0',
					analysis_salary: '0',
					analysis_payFrequency: 'weekly',
					analysis_employerMatchPercentage: '0.0',
					analysis_userContributionType: 'fixed',
					analysis_userContributionAmount: '0.0',
				};

				expect(schema.isValidSync(data)).toBeTruthy();
			});

			test('standard401k_Valid: percentage contribution', () => {
				const schema = vehicleFormSchema;
				const data = {
					investmentVehicleName: 'Test',
					investmentVehicleType: 'Vehicle401k',
					cashHoldings: '0',
					analysis_analysisLength: '60',
					analysis_shortTermCapitalGainsTax: '0.0',
					analysis_longTermCapitalGainsTax: '0.0',
					analysis_salary: '0',
					analysis_payFrequency: 'weekly',
					analysis_employerMatchPercentage: '0.0',
					analysis_userContributionType: 'percentage',
					analysis_userContributionPercentage: '0.0',
				};

				expect(schema.isValidSync(data)).toBeTruthy();
			});
		});

		describe('Other Vehicle Types', () => {
			test('standardOtherVehicle_Valid', () => {
				const schema = vehicleFormSchema;
				const data = {
					investmentVehicleName: 'Test',
					investmentVehicleType: 'VehicleIRA',
					cashHoldings: '0',
					analysis_analysisLength: '60',
					analysis_shortTermCapitalGainsTax: '0.0',
					analysis_longTermCapitalGainsTax: '0.0',
					analysis_userContributionAmount: '0.0',
				};
				expect(schema.isValidSync(data)).toBeTruthy();
			});
		});
	});
});
