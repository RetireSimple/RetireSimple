import dayjs from 'dayjs';

export const convertDates = (data: {[key: string]: string}) => {
	if (data['stockPurchaseDate']) {
		data['stockPurchaseDate'] = dayjs(data['stockPurchaseDate']).format('YYYY-MM-DD');
	}
	if (data['stockDividendFirstPaymentDate']) {
		data['stockDividendFirstPaymentDate'] = dayjs(data['stockDividendFirstPaymentDate']).format(
			'YYYY-MM-DD',
		);
	}
	if (data['bondPurchaseDate']) {
		data['bondPurchaseDate'] = dayjs(data['bondPurchaseDate']).format('YYYY-MM-DD');
	}
	if (data['bondMaturityDate']) {
		data['bondMaturityDate'] = dayjs(data['bondMaturityDate']).format('YYYY-MM-DD');
	}
};
