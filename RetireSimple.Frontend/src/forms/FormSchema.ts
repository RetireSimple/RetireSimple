import {number, object, string} from 'yup';

export const decimalValidation = (maxLen: number, value: number) =>
	value === 0 || value
		? new RegExp(`^\\d+(\\.\\d{1,${maxLen}})?$`).test(value.toString())
		: false;

const decimalErrorString = 'Must be a decimal';

export const investmentFormSchema = object().shape({
	investmentName: string().defined('Required').required(),
	investmentType: string()
		.defined('Required')
		.oneOf(['StockInvestment', 'BondInvestment', 'PensionInvestment'])
		.required(),
	analysisType: string()
		.required()
		.when('investmentType', {
			is: 'StockInvestment',
			then: (schema) =>
				schema.defined('Required').oneOf(['MonteCarlo', 'BinomialRegression']),
		})
		.when('investmentType', {
			is: 'BondInvestment',
			then: (schema) => schema.defined('Required').oneOf(['StdBondValuation']),
		})
		.when('investmentType', {
			is: 'PensionInvestment',
			then: (schema) => schema.defined('Required').oneOf(['PensionSimulation']),
		}),

	//========================================
	// StockInvestment Specific Fields
	//========================================
	stockTicker: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.required('Required')
				.min(1)
				.max(5)
				.uppercase('Must be uppercase'),
		otherwise: (schema) => schema.strip(),
	}),

	stockPrice: number().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.min(0, 'Must be positive')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(2, value),
				)
				.required('Required')
				.typeError('Must be a number'),

		otherwise: (schema) => schema.strip(),
	}),
	stockQuantity: number().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.positive('Must be positive')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(4, value),
				)
				.required('Required')
				.typeError('Must be a number'),
		otherwise: (schema) => schema.strip(),
	}),
	stockPurchaseDate: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) => schema.defined('Required').required('Required'),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendPercent: number().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(4, value),
				)
				.required('Required')
				.typeError('Must be a number'),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendDistributionInterval: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) =>
			schema.defined('Required').oneOf(['Month', 'Quarter', 'Annual']).required('Required'),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendDistributionMethod: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) =>
			schema.defined('Required').oneOf(['Stock', 'Cash', 'DRIP']).required('Required'),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendFirstPaymentDate: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) => schema.defined('Required').required('Required'),
		otherwise: (schema) => schema.strip(),
	}),

	//========================================
	// Bond Investment Specific Fields
	//========================================
	bondTicker: string().when('investmentType', {
		is: 'BondInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.required('Required')
				.min(1)
				.max(5)
				.uppercase('Must be uppercase'),
		otherwise: (schema) => schema.strip(),
	}),
	bondCouponRate: number().when('investmentType', {
		is: 'BondInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.positive('Must be positive')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(4, value),
				)
				.required('Required')
				.typeError('Must be a number'),
		otherwise: (schema) => schema.strip(),
	}),
	bondYieldToMaturity: number().when('investmentType', {
		is: 'BondInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				// .positive('Must be positive')
				.min(0, 'Must be positive')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(4, value),
				)
				.required('Required')
				.typeError('Must be a number'),
		otherwise: (schema) => schema.strip(),
	}),

	bondPurchaseDate: string().when('investmentType', {
		is: 'BondInvestment',
		then: (schema) => schema.defined('Required').required('Required'),
		otherwise: (schema) => schema.strip(),
	}),
	bondMaturityDate: string().when('investmentType', {
		is: 'BondInvestment',
		then: (schema) => schema.defined('Required').required('Required'),
		otherwise: (schema) => schema.strip(),
	}),
	bondCurrentPrice: number().when('investmentType', {
		is: 'BondInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.positive('Must be positive')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(2, value),
				)
				.required('Required')
				.typeError('Must be a number'),
		otherwise: (schema) => schema.strip(),
	}),
	bondFaceValue: number().when('investmentType', {
		is: 'BondInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.positive('Must be positive')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(2, value),
				)
				.required('Required')
				.typeError('Must be a number'),
		otherwise: (schema) => schema.strip(),
	}),

	analysis_isAnnual: string().when('analysisType', {
		is: (value: string) => value === 'StdBondValuation',
		then: (schema) => schema.defined().required().oneOf(['true', 'false']),
		otherwise: (schema) => schema.strip(),
	}),
	//========================================
	// Pension Investment Specific Fields
	//========================================
	pensionStartDate: string().when('investmentType', {
		is: 'PensionInvestment',
		then: (schema) => schema.defined('Required').required('Required'),
		otherwise: (schema) => schema.strip(),
	}),
	pensionInitialMonthlyPayment: number().when('investmentType', {
		is: 'PensionInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.required('Required')
				.positive('Must be positive')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(2, value),
				),
		otherwise: (schema) => schema.strip(),
	}),
	pensionYearlyIncrease: number().when('investmentType', {
		is: 'PensionInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.required('Required')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(2, value),
				),
		otherwise: (schema) => schema.strip(),
	}),
	analysis_expectedTaxRate: number().when('investmentType', {
		is: 'PensionInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.required('Required')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(2, value),
				),
		otherwise: (schema) => schema.strip(),
	}),

	//========================================
	// General Stock Analysis Specific Fields
	//========================================
	analysis_analysisLength: number().defined('Required').positive().required('Required'),
	analysis_analysisPreset: string().when('analysisType', {
		is: (value: string) => ['MonteCarlo', 'BinomialRegression'].includes(value),
		then: (schema) => schema.defined('Required').required('Required'),
		otherwise: (schema) => schema.strip(),
	}),
	//========================================
	// Monte Carlo Analysis Specific Fields
	//========================================
	analysis_simCount: number().when(['analysisType', 'analysis_analysisPreset'], {
		is: (analysisType: string, analysis_analysisPreset: string) =>
			analysisType === 'MonteCarlo' && analysis_analysisPreset === 'Custom',
		then: (schema) => schema.defined('Required').positive().required('Required'),
		otherwise: (schema) => schema.strip(),
	}),
	analysis_randomVariableType: string().when(['analysisType', 'analysis_analysisPreset'], {
		is: (analysisType: string, analysis_analysisPreset: string) =>
			analysisType === 'MonteCarlo' && analysis_analysisPreset === 'Custom',
		then: (schema) =>
			schema.defined('Required').oneOf(['Normal', 'LogNormal']).required('Required'),
		otherwise: (schema) => schema.strip(),
	}),
	analysis_randomVariableMu: number().when(['analysisType', 'analysis_analysisPreset'], {
		is: (analysisType: string, analysis_analysisPreset: string) =>
			analysisType === 'MonteCarlo' && analysis_analysisPreset === 'Custom',
		then: (schema) =>
			schema
				.defined('Required')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(4, value),
				)
				.required('Required'),
		otherwise: (schema) => schema.strip(),
	}),
	analysis_randomVariableSigma: number().when(['analysisType', 'analysis_analysisPreset'], {
		is: (analysisType: string, analysis_analysisPreset: string) =>
			analysisType === 'MonteCarlo' && analysis_analysisPreset === 'Custom',

		then: (schema) =>
			schema
				.defined('Required')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(4, value),
				)
				.required('Required'),
		otherwise: (schema) => schema.strip(),
	}),
	analysis_randomVariableScaleFactor: number().when(['analysisType', 'analysis_analysisPreset'], {
		is: (analysisType: string, analysis_analysisPreset: string) =>
			analysisType === 'MonteCarlo' && analysis_analysisPreset === 'Custom',
		then: (schema) =>
			schema
				.defined('Required')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(4, value),
				)
				.required('Required'),
		otherwise: (schema) => schema.strip(),
	}),

	//========================================
	// Binomial Regression Specific Fields
	//========================================
	analysis_percentGrowth: number().when(['analysisType', 'analysis_analysisPreset'], {
		is: (analysisType: string, analysis_analysisPreset: string) =>
			analysisType === 'BinomialRegression' && analysis_analysisPreset === 'Custom',
		then: (schema) =>
			schema
				.defined('Required')
				.positive('Must be positive')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(4, value),
				)
				.required('Required'),
		otherwise: (schema) => schema.strip(),
	}),
	analysis_uncertainty: number().when(['analysisType', 'analysis_analysisPreset'], {
		is: (analysisType: string, analysis_analysisPreset: string) =>
			analysisType === 'BinomialRegression' && analysis_analysisPreset === 'Custom',
		then: (schema) =>
			schema
				.defined('Required')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(4, value),
				)
				.required('Required'),
		otherwise: (schema) => schema.strip(),
	}),
});

export const InvestmentFormDefaults = {
	investmentName: '',
	investmentType: 'StockInvestment',
	stockTicker: '',
	stockPurchaseDate: '',
	stockPrice: '',
	stockQuantity: '',
	stockDividendPercent: '',
	stockDividendDistributionInterval: '',
	stockDividendDistributionMethod: '',
	stockDividendFirstPaymentDate: '',
	analysisType: 'MonteCarlo',
	analysis_analysisPreset: 'DefaultStockAnalysis',
	analysis_analysisLength: '60',
	analysis_randomVariableType: 'Normal',
	analysis_simCount: '10000',
	analysis_randomVariableMu: '0.0',
	analysis_randomVariableSigma: '1.0',
	analysis_randomVariableScaleFactor: '1',
};

export const vehicleFormSchema = object().shape({
	investmentVehicleName: string().defined('Required').required('Required').max(50),
	investmentVehicleType: string()
		.defined('Required')
		.required('Required')
		.oneOf(['Vehicle401k', 'VehicleIRA', 'VehicleRothIRA', 'Vehicle403b', 'Vehicle457']),
	cashHoldings: number()
		.defined('Required')
		.required('Required')
		.test('is-decimal', decimalErrorString, (value) =>
			isNaN(value) ? false : decimalValidation(2, value),
		),

	//========================================
	// Analysis Related Fields
	//========================================
	analysis_analysisLength: number()
		.defined('Required')
		.positive('Must be Positive')
		.required('Required'),
	analysis_shortTermCapitalGainsTax: number()
		.defined('Required')
		.required('Required')
		.test('is-decimal', decimalErrorString, (value) =>
			isNaN(value) ? false : decimalValidation(2, value),
		),
	analysis_longTermCapitalGainsTax: number()
		.defined('Required')
		.required('Required')
		.test('is-decimal', decimalErrorString, (value) =>
			isNaN(value) ? false : decimalValidation(2, value),
		),

	//========================================
	// 401k Specific Fields
	//========================================
	analysis_salary: number().when('investmentVehicleType', {
		is: (val: string) => ['Vehicle401k', 'Vehicle403b', 'Vehicle457'].includes(val),
		then: (schema) =>
			schema
				.defined('Required')
				.required('Required')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(2, value),
				),

		otherwise: (schema) => schema.strip(),
	}),
	analysis_payFrequency: string().when('investmentVehicleType', {
		is: (val: string) => ['Vehicle401k', 'Vehicle403b', 'Vehicle457'].includes(val),
		then: (schema) =>
			schema
				.defined('Required')
				.required('Required')
				.oneOf(['weekly', 'biweekly', 'monthly']),
		otherwise: (schema) => schema.strip(),
	}),
	analysis_employerMatchPercentage: number().when('investmentVehicleType', {
		is: (val: string) => ['Vehicle401k', 'Vehicle403b', 'Vehicle457'].includes(val),
		then: (schema) =>
			schema
				.defined('Required')
				.required('Required')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(2, value),
				),
		otherwise: (schema) => schema.strip(),
	}),
	analysis_userContributionType: string().when('investmentVehicleType', {
		is: (val: string) => ['Vehicle401k', 'Vehicle403b', 'Vehicle457'].includes(val),
		then: (schema) =>
			schema.defined('Required').required('Required').oneOf(['fixed', 'percentage']),
		otherwise: (schema) => schema.strip(),
	}),
	analysis_userContributionAmount: number().when(
		['investmentVehicleType', 'analysis_userContributionType'],
		{
			is: (investmentVehicleType: string, analysis_userContributionType: string) =>
				(['Vehicle401k', 'Vehicle403b', 'Vehicle457'].includes(investmentVehicleType) &&
					analysis_userContributionType === 'fixed') ||
				!['Vehicle401k', 'Vehicle403b', 'Vehicle457'].includes(investmentVehicleType),
			then: (schema) =>
				schema
					.defined('Required')
					.required('Required')
					.test('is-decimal', decimalErrorString, (value) =>
						isNaN(value) ? false : decimalValidation(2, value),
					),
			otherwise: (schema) => schema.strip(),
		},
	),
	analysis_userContributionPercentage: number().when(
		['investmentVehicleType', 'analysis_userContributionType'],
		{
			is: (investmentVehicleType: string, analysis_userContributionType: string) =>
				['Vehicle401k', 'Vehicle403b', 'Vehicle457'].includes(investmentVehicleType) &&
				analysis_userContributionType === 'percentage',
			then: (schema) =>
				schema
					.defined('Required')
					.required('Required')
					.test('is-decimal', decimalErrorString, (value) =>
						isNaN(value) ? false : decimalValidation(2, value),
					),
			otherwise: (schema) => schema.strip(),
		},
	),
});

export const VehicleFormDefaults = {
	investmentVehicleName: '',
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
	analysis_userContributionPercentage: '0.0',
};
