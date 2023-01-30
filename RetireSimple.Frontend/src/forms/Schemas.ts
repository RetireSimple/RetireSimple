import {number, object, string} from 'yup';

export const investmentFormSchema = object().shape({
	investmentName: string().defined().required(),
	investmentType: string().defined().oneOf(['StockInvestment', 'BondInvestment']).required(),
	analysisType: string()
		.required()
		.when('investmentType', {
			is: 'StockInvestment',
			then: (schema) =>
				schema
					.defined()
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
		then: (schema) => schema.defined().required().min(3).max(5).uppercase(),
		otherwise: (schema) => schema.strip(),
	}),

	stockPrice: number().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) =>
			schema
				.defined()
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
				.defined()
				.positive()
				.test('is-decimal', 'Quantity must be a decimal', (value) =>
					value ? /^\d+(\.\d{1,2})?$/.test(value.toString()) : false,
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockPurchaseDate: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) => schema.defined().required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendPercent: number().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) =>
			schema
				.defined()
				.positive()
				.test('is-decimal', 'Dividend Percent must be a decimal', (value) =>
					value ? /^\d+(\.\d{1,4})?$/.test(value.toString()) : false,
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendDistributionInterval: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) => schema.defined().oneOf(['Month', 'Quarter', 'Annual']).required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendDistributionMethod: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) => schema.defined().oneOf(['Stock', 'Cash', 'DRIP']).required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendFirstPaymentDate: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) => schema.defined().required(),
		otherwise: (schema) => schema.strip(),
	}),

	//========================================
	// Bond Investment Specific Fields
	//========================================
	//todo - add bond specific fields

	//========================================
	// Monte Carlo Analysis Specific Fields
	//========================================
	analysisLength: number().when('analysisType', {
		is: (value: string) => value !== 'testAnalysis',
		then: (schema) => schema.defined().positive().required(),
	}),
	simCount: number().when('analysisType', {
		is: (value: string) =>
			value === 'MonteCarlo_NormalDist' || value === 'MonteCarlo_LogNormalDist',
		then: (schema) => schema.defined().positive().required(),
		otherwise: (schema) => schema.strip(),
	}),
	randomVariableMu: number().when('analysisType', {
		is: (value: string) =>
			value === 'MonteCarlo_NormalDist' || value === 'MonteCarlo_LogNormalDist',
		then: (schema) =>
			schema
				.defined()
				.test('is-decimal', 'Random Variable Mu must be a decimal', (value) =>
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
				.defined()
				.test('is-decimal', 'Random Variable Sigma must be a decimal', (value) =>
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
				.defined()
				.test('is-decimal', 'Random Variable Sigma must be a decimal', (value) =>
					value ? /^\d+(\.\d{1,4})?$/.test(value.toString()) : false,
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
});
