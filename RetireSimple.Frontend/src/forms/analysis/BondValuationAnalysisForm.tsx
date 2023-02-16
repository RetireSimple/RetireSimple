import {Grid, Typography} from '@mui/material';
import React from 'react';
import {useFormContext} from 'react-hook-form';
import {FormTextField} from '../Inputs';

export const BondValuationAnalysisForm = () => {
	const formContext = useFormContext();
	const {errors} = formContext.formState;

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================
	const analysisLengthField = (
		<FormTextField
			name='analysis_analysisLength'
			label='Analysis Length (Months)'
			control={formContext.control}
			errorField={errors.analysisLength}
		/>);

	return (
		<Grid container spacing={2}>
			<Grid item xs={12}>
				<Typography variant='subtitle2'>Bond Valuation Parameters</Typography>
			</Grid>
			<Grid item xs={4}>{analysisLengthField}</Grid>
			<Grid item xs={12}>
				<Typography variant='subtitle2'> s ss s </Typography>
			</Grid>
		</Grid>
	);
};
