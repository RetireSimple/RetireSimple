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

const percentageFields = [
	'stockDividendPercent',
	'bondCoupunRate',
	'bondYieldToMaturity',
	'analysis_userContributionPercentage',
	'analysis_employerMatchPercentage',
	'analysis_shortTermCapitalGainsTax',
	'analysis_longTermCapitalGainsTax',
];

export const convertFromDecimal = (data: {[key: string]: string}) => {
	percentageFields.forEach((field) => {
		if (data[field]) {
			data[field] = (parseFloat(data[field]) * 100).toPrecision(4).toString();
			while (data[field].endsWith('0')) {
				data[field] = data[field].slice(0, -1);
			}
			if (data[field].endsWith('.')) {
				data[field] = data[field].slice(0, -1);
			}
		}
	});
};

export const convertToDecimal = (data: {[key: string]: string}) => {
	percentageFields.forEach((field) => {
		if (data[field]) {
			//Do a workaround to prevent JS rounding errors
			data[field] = (parseFloat(data[field]) / 100).toPrecision(4).toString();
			while (data[field].endsWith('0')) {
				data[field] = data[field].slice(0, -1);
			}
			if (data[field].endsWith('.')) {
				data[field] = data[field].slice(0, -1);
			}
		}
	});
};

export const addSpacesCapitalCase = (str: string) => {
	return str.replace(/([A-Z])/g, ' $1').replace(/^./, (str) => str.toUpperCase());
};
