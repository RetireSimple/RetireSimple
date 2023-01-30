import {number, object, string} from 'yup';

export const investmentFormSchema = object().shape({
	investmentName: string().required(),
	investmentType: string().oneOf(['StockInvestment', 'BondInvestment']).required(),
	analysisType: string()
		.required()
		.when('investmentType', {
			is: 'StockInvestment',
			then: (schema) =>
				schema.oneOf(['MonteCarlo_NormalDist', 'MonteCarlo_LogNormalDist', 'testAnalysis']),
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
		then: (schema) => schema.required().min(3).max(5).uppercase(),
		otherwise: (schema) => schema.strip(),
	}),

	stockPrice: number().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) =>
			schema
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
				.positive()
				.test('is-decimal', 'Quantity must be a decimal', (value) =>
					value ? /^\d+(\.\d{1,2})?$/.test(value.toString()) : false,
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockPurchaseDate: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) => schema.required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendPercent: number().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) =>
			schema
				.positive()
				.test('is-decimal', 'Dividend Percent must be a decimal', (value) =>
					value ? /^\d+(\.\d{1,4})?$/.test(value.toString()) : false,
				)
				.required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendDistributionInterval: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) => schema.oneOf(['Month', 'Quarter', 'Annual']).required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendDistributionMethod: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) => schema.oneOf(['Stock', 'Cash', 'DRIP']).required(),
		otherwise: (schema) => schema.strip(),
	}),
	stockDividendFirstPaymentDate: string().when('investmentType', {
		is: 'StockInvestment',
		then: (schema) => schema.required(),
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
		then: (schema) => schema.positive().required(),
	}),

});
