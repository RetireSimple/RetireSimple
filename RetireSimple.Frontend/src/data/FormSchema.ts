import {number, object, string} from 'yup';

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
			then: (schema) => schema.oneOf(['MonteCarloAnalysis']), //todo - add bond specific analysis types
		}),

	//========================================
	// StockInvestment Specific Fields
	//========================================
	stockTicker: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) => schema.defined('Required').required().min(3).max(5).uppercase(),
		otherwise: (schema) => schema.strip(),
	}),

	stockPrice: number().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.positive()
				.test('is-decimal', 'Price must be a decimal', (value) =>
					value ? /^\d+(\.\d{1,2})?$/.test(value.toString()) : false,
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockQuantity: number().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.positive()
				.test('is-decimal', 'Quantity must be a decimal', (value) =>
					value ? /^\d+(\.\d{1,2})?$/.test(value.toString()) : false,
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockPurchaseDate: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) => schema.defined('Required').required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendPercent: number().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.positive()
				.test('is-decimal', 'Dividend Percent must be a decimal', (value) =>
					value ? /^\d+(\.\d{1,4})?$/.test(value.toString()) : false,
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendDistributionInterval: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) =>
			schema.defined('Required').oneOf(['Month', 'Quarter', 'Annual']).required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendDistributionMethod: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) => schema.defined('Required').oneOf(['Stock', 'Cash', 'DRIP']).required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendFirstPaymentDate: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) => schema.defined('Required').required(),
		otherwise: (schema) => schema.strip(),
	}),

	//========================================
	// Bond Investment Specific Fields
	//========================================
	//TODO modify engine defs to include
	// bondTicker: string().when('investmentType', {
	// 	is: 'BondInvestment',
	// 	then: (schema) => schema.defined('Required').required().min(3).max(5).uppercase(),
	// 	otherwise: (schema) => schema.strip(),
	// }),
	bondCouponRate: number().when('investmentType', {
		is: 'BondInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.positive()
				.test('is-decimal', 'Coupon Rate must be a decimal', (value) =>
					value ? /^\d+(\.\d{1,4})?$/.test(value.toString()) : false,
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
	bondPurchaseDate: string().when('investmentType', {
		is: 'BondInvestment',
		then: (schema) => schema.defined('Required').required(),
		otherwise: (schema) => schema.strip(),
	}),
	bondMaturityDate: string().when('investmentType', {
		is: 'BondInvestment',
		then: (schema) => schema.defined('Required').required(),
		otherwise: (schema) => schema.strip(),
	}),
	bondFaceValue: number().when('investmentType', {
		is: 'BondInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.positive()
				.test('is-decimal', 'Face Value must be a decimal', (value) =>
					value ? /^\d+(\.\d{1,2})?$/.test(value.toString()) : false,
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
	bondPurchasePrice: number().when('investmentType', {
		is: 'BondInvestment',
		then: (schema) =>
			schema
				.defined('Required')
				.positive()
				.test('is-decimal', 'Purchase Price must be a decimal', (value) =>
					value ? /^\d+(\.\d{1,2})?$/.test(value.toString()) : false,
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),

	//========================================
	// Monte Carlo Analysis Specific Fields
	//========================================
	analysisLength: number().when('analysisType', {
		is: (value: string) => value !== 'testAnalysis',
		then: (schema) => schema.defined('Required').positive().required(),
		otherwise: (schema) => schema.strip(),
	}),
	simCount: number().when('analysisType', {
		is: (value: string) =>
			value === 'MonteCarlo_NormalDist' || value === 'MonteCarlo_LogNormalDist',
		then: (schema) => schema.defined('Required').positive().required(),
		otherwise: (schema) => schema.strip(),
	}),
	randomVariableMu: number().when('analysisType', {
		is: (value: string) =>
			value === 'MonteCarlo_NormalDist' || value === 'MonteCarlo_LogNormalDist',
		then: (schema) =>
			schema
				.defined('Required')
				.test('is-decimal', 'Must be a decimal', (value) =>
					value ? /^\d+(\.\d{1,4})?$/.test(value.toString()) : false,
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
	randomVariableSigma: number().when('analysisType', {
		is: (value: string) =>
			value === 'MonteCarlo_NormalDist' || value === 'MonteCarlo_LogNormalDist',
		then: (schema) =>
			schema
				.defined('Required')
				.test('is-decimal', 'Must be a decimal', (value) =>
					value ? /^\d+(\.\d{1,4})?$/.test(value.toString()) : false,
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
	randomVariableScaleFactor: number().when('analysisType', {
		is: (value: string) =>
			value === 'MonteCarlo_NormalDist' || value === 'MonteCarlo_LogNormalDist',
		then: (schema) =>
			schema
				.defined('Required')
				.test('is-decimal', 'Must be a decimal', (value) =>
					value ? /^\d+(\.\d{1,4})?$/.test(value.toString()) : false,
				)
				.required(),
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
	analysisLength: '60',
	simCount: '10000',
	randomVariableMu: '0.0',
	randomVariableSigma: '1.0',
	randomVariableScaleFactor: '1',
};
