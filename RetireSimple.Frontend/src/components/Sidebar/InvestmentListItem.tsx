import {Box, Typography} from '@mui/material';
import React from 'react';
import {Investment} from '../../models/Interfaces';

export interface InvestmentListItemProps {
	investmentName: string;
	investmentNumberValue: number;	//The raw dollar value
	investmentValue: string;	//Can be different depending on the investment given
	investmentTicker?: string;
	investmentType: string;
	investmentId: number;

}

export const mapListItemProps = (investment: Investment) => {
	switch (investment.investmentType) {
		case 'StockInvestment':
			return {
				investmentName: investment.investmentName,
				investmentNumberValue: (Number.parseFloat(investment.investmentData["stockPrice"])
					* Number.parseFloat(investment.investmentData["stockQuantity"])),
				investmentValue: `${investment.investmentData["stockQuantity"]} @ $${investment.investmentData["stockPrice"]}`,
				investmentTicker: investment.investmentData["stockTicker"],
				investmentType: investment.investmentType,
				investmentId: investment.investmentId,
			};
		default:
			return {
				investmentName: "Unknown Investment",
				investmentNumberValue: 0,
				investmentValue: "Unknown Investment",
				investmentTicker: "Unknown Investment",
				investmentType: "Unknown Investment",
				investmentId: investment.investmentId,
			};
	}
};

export const InvestmentListItem = (props: InvestmentListItemProps) => {
	return (
		<Box>
			<Typography variant="body1" component="div" sx={{flexGrow: 1}}>
				{props.investmentName}
			</Typography>
			<Typography variant='body2' component='div' sx={{flexGrow: 1}}>
				Type: {props.investmentType +
					(props.investmentTicker ? ` (${props.investmentTicker})` : '')}
			</Typography>
			<Typography variant='body2' component='div' sx={{flexGrow: 1}}>
				{props.investmentValue + "-> $" + props.investmentNumberValue}
			</Typography>
		</Box>
	);
};
