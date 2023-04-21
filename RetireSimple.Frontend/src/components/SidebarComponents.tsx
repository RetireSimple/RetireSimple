import {Box, MenuItem, Typography} from '@mui/material';
import {Link} from 'react-router-dom';
import {Investment} from '../Interfaces';

export interface InvestmentListItemProps {
	investmentName: string;
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
				investmentTicker: investment.investmentData['bondTicker'],
				investmentType: 'Bond',
				investmentId: investment.investmentId,
			};
		case 'StockInvestment':
			return {
				investmentName: investment.investmentName,
				investmentTicker: investment.investmentData['stockTicker'],
				investmentType: 'Stock',
				investmentId: investment.investmentId,
			};
		case 'PensionInvestment':
			return {
				investmentName: investment.investmentName,
				investmentType: 'Pension',
				investmentId: investment.investmentId,
			};
		default:
			return {
				investmentName: 'Unknown Investment',
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
				{props.investmentType +
					(props.investmentTicker ? ` (${props.investmentTicker})` : '')}
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
