import dayjs from 'dayjs';
import {convertDates, convertFromDecimal, convertToDecimal} from '../api/ConvertUtils';

describe('convertDates', () => {
	test('convert stockPurchaseDate', () => {
		const data = {stockPurchaseDate: dayjs('2021-03-01').toISOString()};
		convertDates(data);
		expect(data.stockPurchaseDate).toEqual('2021-03-01');
	});

	test('convert stockDividendFirstPaymentDate', () => {
		const data = {stockDividendFirstPaymentDate: dayjs('2021-03-01').toISOString()};
		convertDates(data);
		expect(data.stockDividendFirstPaymentDate).toEqual('2021-03-01');
	});

	test('convert bondPurchaseDate', () => {
		const data = {bondPurchaseDate: dayjs('2021-03-01').toISOString()};
		convertDates(data);
		expect(data.bondPurchaseDate).toEqual('2021-03-01');
	});

	test('convert bondMaturityDate', () => {
		const data = {bondMaturityDate: dayjs('2021-03-01').toISOString()};
		convertDates(data);
		expect(data.bondMaturityDate).toEqual('2021-03-01');
	});

	test('convert all dates', () => {
		const data = {
			stockPurchaseDate: dayjs('2021-03-01').toISOString(),
			stockDividendFirstPaymentDate: dayjs('2019-04-05').toISOString(),
			bondPurchaseDate: dayjs('2021-12-25').toISOString(),
			bondMaturityDate: dayjs('2023-03-16').toISOString(),
		};
		convertDates(data);
		expect(data.stockPurchaseDate).toEqual('2021-03-01');
		expect(data.stockDividendFirstPaymentDate).toEqual('2019-04-05');
		expect(data.bondPurchaseDate).toEqual('2021-12-25');
		expect(data.bondMaturityDate).toEqual('2023-03-16');
	});

	test('convert no dates', () => {
		const data = {};
		convertDates(data);
		expect(data).toEqual({});
	});

	test('does not touch unknown fields', () => {
		const data = {foo: 'bar'};
		convertDates(data);
		expect(data).toEqual({foo: 'bar'});
	});
});

describe('convertFromDecimal', () => {
	test('converts known percentage fields', () => {
		const data = {
			stockDividendPercent: '0.101',
			bondCoupunRate: '0.2',
			bondYieldToMaturity: '0.3',
			analysis_userContributionPercentage: '0.4',
			analysis_employerMatchPercentage: '0.5',
		};
		convertFromDecimal(data);
		expect(data.stockDividendPercent).toEqual('10.1');
		expect(data.bondCoupunRate).toEqual('20');
		expect(data.bondYieldToMaturity).toEqual('30');
		expect(data.analysis_userContributionPercentage).toEqual('40');
		expect(data.analysis_employerMatchPercentage).toEqual('50');
	});

	test('does not touch unknown fields', () => {
		const data = {foo: 'bar'};
		convertFromDecimal(data);
		expect(data).toEqual({foo: 'bar'});
	});
});

describe('convertToDecimal', () => {
	test('converts known percentage fields', () => {
		const data = {
			stockDividendPercent: '10.1',
			bondCoupunRate: '20',
			bondYieldToMaturity: '30',
			analysis_userContributionPercentage: '40',
			analysis_employerMatchPercentage: '50',
		};
		convertToDecimal(data);
		expect(data.stockDividendPercent).toEqual('0.101');
		expect(data.bondCoupunRate).toEqual('0.2');
		expect(data.bondYieldToMaturity).toEqual('0.3');
		expect(data.analysis_userContributionPercentage).toEqual('0.4');
		expect(data.analysis_employerMatchPercentage).toEqual('0.5');
	});

	test('does not touch unknown fields', () => {
		const data = {foo: 'bar'};
		convertToDecimal(data);
		expect(data).toEqual({foo: 'bar'});
	});
});
