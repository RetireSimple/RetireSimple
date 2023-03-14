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
		.oneOf(['StockInvestment', 'BondInvestment'])
		.required(),
	analysisType: string()
		.required()
		.when('investmentType', {
			is: 'StockInvestment',
			then: (schema) =>
				schema
					.defined('Required')
					.oneOf(['MonteCarlo_NormalDist', 'MonteCarlo_LogNormalDist', 'testAnalysis']),
		})
		.when('investmentType', {
			is: 'BondInvestment',
			then: (schema) => schema.oneOf(['bondValuationAnalysis']), //todo - add bond specific analysis types
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
				.min(3)
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
				.positive('Must be positive')
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
				.min(3)
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

	//========================================
	// Monte Carlo Analysis Specific Fields
	//========================================
	analysis_analysisLength: number().when('analysisType', {
		is: (value: string) => value !== 'testAnalysis',
		then: (schema) => schema.defined('Required').positive().required(),
		otherwise: (schema) => schema.strip(),
	}),
	analysis_simCount: number().when('analysisType', {
		is: (value: string) =>
			value === 'MonteCarlo_NormalDist' || value === 'MonteCarlo_LogNormalDist',
		then: (schema) => schema.defined('Required').positive().required(),
		otherwise: (schema) => schema.strip(),
	}),
	analysis_randomVariableMu: number().when('analysisType', {
		is: (value: string) =>
			value === 'MonteCarlo_NormalDist' || value === 'MonteCarlo_LogNormalDist',
		then: (schema) =>
			schema
				.defined('Required')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(4, value),
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
	analysis_randomVariableSigma: number().when('analysisType', {
		is: (value: string) =>
			value === 'MonteCarlo_NormalDist' || value === 'MonteCarlo_LogNormalDist',
		then: (schema) =>
			schema
				.defined('Required')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(4, value),
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
	analysis_randomVariableScaleFactor: number().when('analysisType', {
		is: (value: string) =>
			value === 'MonteCarlo_NormalDist' || value === 'MonteCarlo_LogNormalDist',
		then: (schema) =>
			schema
				.defined('Required')
				.test('is-decimal', decimalErrorString, (value) =>
					isNaN(value) ? false : decimalValidation(4, value),
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
	analysis_isAnnual: string().when('analysisType', {
		is: (value: string) => value === 'bondValuationAnalysis',
		then: (schema) => schema.defined().required().oneOf(['true', 'false']),
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
	analysisType: 'MonteCarlo_NormalDist',
	analysis_analysisLength: '60',
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
		.oneOf(['401k', 'IRA', 'RothIRA', '403b', '457']),
	cashHoldings: number().defined('Required').positive('Must be positive'),

	//========================================
	// Analysis Related Fields
	//========================================
	analysis_analysisLength: number().defined('Required').positive().required('Required'),
	analysis_cashContribution: number()
		.defined('Required')
		.positive('Must be positive')
		.required('Required'),
	analysis_vehicleTaxPercentage: number().defined('Required').positive('Must be positive'),
});

export const VehicleFormDefaults = {
	investmentVehicleName: '',
	investmentvehicleType: '401k',
	cashHoldings: '',
	analysis_analysisLength: '60',
	analysis_cashContribution: '',
	analysis_vehicleTaxPercentage: '',
};
