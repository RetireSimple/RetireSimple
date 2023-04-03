import {Box, Grid, Typography} from '@mui/material';
import React from 'react';
import {useFormContext} from 'react-hook-form';
import {
	FormDatePicker,
	FormTextField,
	FormTextFieldCurrency,
	FormTextFieldPercent,
} from '../../components/InputComponents';
import {BondValuationAnalysisForm} from '../analysis/BondValuationAnalysisForm';

export interface BondFormProps {
	defaultValues?: any;
	analyisisTypeField: React.ReactNode;
}

export const BondForm = (props: BondFormProps) => {
	const formContext = useFormContext();
	const {errors} = formContext.formState;

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================
	const bondTickerField = (
		<FormTextField
			name='bondTicker'
			label='Ticker'
			control={formContext.control}
			errorField={errors.bondTicker}
			tooltip={
				<>
					<Typography variant='inherit'>The ticker symbol for this bond.</Typography>
					<Typography variant='inherit'>
						This is primarily used as another identifier for the bond.
					</Typography>
				</>
			}
		/>
	);

	const bondCouponRateField = (
		<FormTextFieldPercent
			name='bondCouponRate'
			label='Coupon Rate'
			control={formContext.control}
			errorField={errors.bondCouponRate}
			tooltip={
				<>
					<Typography variant='inherit'>The coupon rate for this bond. </Typography>
					<Typography variant='inherit'>
						This is the annual interest rate that the bond issuer pays to the bond
						holder.'
					</Typography>
				</>
			}
		/>
	);

	const bondYTMField = (
		<FormTextFieldPercent
			name='bondYieldToMaturity'
			label='Yield to Maturity'
			control={formContext.control}
			errorField={errors.bondYieldToMaturity}
			tooltip={
				<>
					<Typography variant='inherit'>The yield to maturity for this bond.</Typography>
					<Typography variant='inherit'>
						This is the annual interest rate that the bond issuer pays to the bond
						holder.
					</Typography>
				</>
			}
		/>
	);

	const bondMaturityDateField = (
		<FormDatePicker
			name='bondMaturityDate'
			label='Maturity Date'
			control={formContext.control}
			errorField={errors.bondMaturityDate}
			defaultValue={props.defaultValues?.bondMaturityDate ?? ''}
			tooltip={
				<>
					<Typography variant='inherit'>The date that the bond will mature.</Typography>
					<Typography variant='inherit'>
						This is the date that the bond issuer will pay the bond holder the face
						value of the bond.
					</Typography>
				</>
			}
		/>
	);

	const bondFaceValueField = (
		<FormTextFieldCurrency
			name='bondFaceValue'
			label='Face Value'
			control={formContext.control}
			errorField={errors.bondFaceValue}
			tooltip='The face value of the bond.'
		/>
	);

	const bondPurchaseDateField = (
		<FormDatePicker
			name='bondPurchaseDate'
			label='Purchase Date'
			control={formContext.control}
			errorField={errors.bondPurchaseDate}
			defaultValue={props.defaultValues?.bondPurchaseDate ?? ''}
			tooltip='The date that the bond was purchased.'
		/>
	);

	const bondCurrentPriceField = (
		<FormTextFieldCurrency
			name='bondCurrentPrice'
			label='Current Price'
			control={formContext.control}
			errorField={errors.bondCurrentPrice}
			tooltip='The current price of the bond.'
		/>
	);

	return (
		<>
			<Box sx={{flexGrow: 1, marginTop: '1rem'}}>
				<Grid container spacing={2}>
					<Grid item xs={12}>
						<Typography variant='subtitle2'>Bond Information</Typography>
					</Grid>
					<Grid item xs={6} sm={4}>
						{bondTickerField}
					</Grid>
					<Grid item xs={6} sm={4}>
						{bondCouponRateField}
					</Grid>
					<Grid item xs={6} sm={4}>
						{bondYTMField}
					</Grid>
					<Grid item xs={6} sm={4}>
						{bondFaceValueField}
					</Grid>
					<Grid item xs={6} sm={4}>
						{bondCurrentPriceField}
					</Grid>
					<Grid item xs={6} sm={4} />
					<Grid item xs={6} sm={4}>
						{bondPurchaseDateField}
					</Grid>
					<Grid item xs={6} sm={4}>
						{bondMaturityDateField}
					</Grid>
					<Grid item xs={6} sm={4} />
					{/* Analysis Section */}
					<Grid item xs={12}>
						<Typography variant='subtitle2'>Analysis Configuration</Typography>
					</Grid>
					<Grid item xs={4}>
						{props.analyisisTypeField}
					</Grid>
					<Grid item xs={12}>
						<BondValuationAnalysisForm />
					</Grid>
				</Grid>
			</Box>
		</>
	);
};
