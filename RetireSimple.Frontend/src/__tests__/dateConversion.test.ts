import dayjs from 'dayjs';
import {convertDates} from '../api/DateUtils';

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
