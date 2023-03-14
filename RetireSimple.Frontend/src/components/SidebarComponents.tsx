import {Box, MenuItem, Typography} from '@mui/material';
import {Link} from 'react-router-dom';
import {Investment} from '../Interfaces';

export interface InvestmentListItemProps {
	investmentName: string;
	investmentNumberValue: string; //The raw dollar value
	investmentValue: string; //Can be different depending on the investment given
	investmentTicker?: string;
	investmentType: string;
	investmentId: number;
}

export interface VehicleListItemProps {
	vehicleName: string;
	vehicleType: string;
}

export const mapListItemProps = (investment: Investment) => {
	switch (investment.investmentType) {
		case 'BondInvestment':
			return {
				investmentName: investment.investmentName,
				investmentNumberValue: Number.parseFloat(
					investment.investmentData['bondFaceValue'],
				).toFixed(2),
				investmentValue: `${investment.investmentData['bondFaceValue']}`,
				investmentTicker: investment.investmentData['bondTicker'],
				investmentType: 'Bond',
				investmentId: investment.investmentId,
			};
		case 'StockInvestment':
			return {
				investmentName: investment.investmentName,
				investmentNumberValue: (
					Number.parseFloat(investment.investmentData['stockPrice']) *
					Number.parseFloat(investment.investmentData['stockQuantity'])
				).toFixed(2),
				investmentValue: `${investment.investmentData['stockQuantity']} @ $${investment.investmentData['stockPrice']}`,
				investmentTicker: investment.investmentData['stockTicker'],
				investmentType: 'Stock',
				investmentId: investment.investmentId,
			};
		default:
			return {
				investmentName: 'Unknown Investment',
				investmentNumberValue: '0',
				investmentValue: 'Unknown Investment',
				investmentTicker: 'Unknown Investment',
				investmentType: 'Unknown Investment',
				investmentId: investment.investmentId,
			};
	}
};

export const InvestmentListItem = (props: InvestmentListItemProps) => {
	return (
		<Box>
			<Typography variant='body1' component='div' sx={{flexGrow: 1, textDecoration: 'none'}}>
				{props.investmentName}
			</Typography>
			<Typography variant='body2' component='div' sx={{flexGrow: 1}}>
				Type:{' '}
				{props.investmentType +
					(props.investmentTicker ? ` (${props.investmentTicker})` : '')}
			</Typography>
			<Typography variant='body2' component='div' sx={{flexGrow: 1}}>
				{props.investmentValue + '-> $' + props.investmentNumberValue}
			</Typography>
		</Box>
	);
};

export const SidebarInvestment = (props: {investment: Investment}) => {
	return (
		<MenuItem component={Link} to={`/investment/${props.investment.investmentId}`}>
			<InvestmentListItem {...mapListItemProps(props.investment)} />
		</MenuItem>
	);
};

export const VehicleListItem = (props: VehicleListItemProps) => {
	return (
		<Box>
			<Typography variant='body1' component='div' sx={{flexGrow: 1, textDecoration: 'none'}}>
				{props.vehicleName}
			</Typography>
			<Typography variant='body2' component='div' sx={{flexGrow: 1}}>
				Type: {props.vehicleType}
			</Typography>
		</Box>
	);
};
