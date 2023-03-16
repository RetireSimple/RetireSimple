import {Grid, Typography} from '@mui/material';
import React from 'react';
import {useFormContext} from 'react-hook-form';
import {FormSelectField, FormTextField} from '../../components/InputComponents';

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
			errorField={errors.analysis_analysisLength}
			tooltip='The number of months starting from today to run the analysis for.'
		/>
	);
	const analysisIsAnnualField = (
		<FormSelectField
			name='analysis_isAnnual'
			label='Is Annual'
			control={formContext.control}
			options={[
				{value: 'true', label: 'Annual'},
				{value: 'false', label: 'Semi Annual'},
			]}
			defaultOption={'true'}
			disable={false}
			errorField={errors.analysis_isAnnual}
		/>
	);

	return (
		<Grid container spacing={2}>
			<Grid item xs={12}>
				<Typography variant='subtitle2'>Bond Valuation Parameters</Typography>
			</Grid>
			<Grid item xs={4}>
				{analysisLengthField}
			</Grid>
			<Grid item xs={4}>
				{analysisIsAnnualField}
			</Grid>
		</Grid>
	);
};
