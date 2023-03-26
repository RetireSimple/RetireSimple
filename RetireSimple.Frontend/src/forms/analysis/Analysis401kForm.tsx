import {Grid, Typography} from '@mui/material';
import React from 'react';
import {useFormContext, useWatch} from 'react-hook-form';
import {FormSelectField, FormTextField} from '../../components/InputComponents';

export const Analysis401kForm = () => {
	const formContext = useFormContext();
	const userContributionType = useWatch({
		name: 'analysis_userContributionType',
		control: formContext.control,
		defaultValue: 'percentage',
	});

	const {errors} = formContext.formState;

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================

	const salaryField = (
		<FormTextField
			name='analysis_salary'
			label='Salary'
			control={formContext.control}
			errorField={errors.analysis_salary}
			tooltip={
				<>
					<Typography variant='inherit'>
						The salary of the job associated with this vehicle.
					</Typography>
					<Typography variant='inherit'>
						This is used to determine the amount of money that can be contributed to the
						vehicle.
					</Typography>
				</>
			}
			decoration='currency'
		/>
	);

	const payFrequencyField = (
		<FormSelectField
			name='analysis_payFrequency'
			label='Pay Frequency'
			control={formContext.control}
			errorField={errors.analysis_payFrequency}
			defaultOption='biweekly'
			options={[
				{value: 'biweekly', label: 'Biweekly'},
				{value: 'weekly', label: 'Weekly'},
				{value: 'monthly', label: 'Monthly'},
			]}
			disable={false}
			tooltip={
				<>
					<Typography variant='inherit'>
						The frequency of paychecks for the job associated with this vehicle.
					</Typography>
					<Typography variant='inherit'>
						This is used to determine monthly contribution amounts and limits.
					</Typography>
				</>
			}
		/>
	);

	const maxEmployerContributionField = (
		<FormTextField
			name='analysis_maxEmployerContributionPercentage'
			label='Max Employer Contribution'
			control={formContext.control}
			errorField={errors.analysis_maxEmployerContributionPercentage}
			tooltip={
				<>
					<Typography variant='inherit'>
						The maximum amount that the employer can contribute to the vehicle.
					</Typography>
					<Typography variant='inherit'>
						This is used to reflect legal limits on employer contributions to 401k
						plans.
					</Typography>
				</>
			}
		/>
	);

	const employerMatchField = (
		<FormTextField
			name='analysis_employerMatchPercentage'
			label='Employer Match'
			control={formContext.control}
			errorField={errors.analysis_employerMatchPercentage}
			tooltip={
				<>
					<Typography variant='inherit'>
						The percentage of the employee contribution that the employer will match.
					</Typography>
				</>
			}
		/>
	);

	const userContributionTypeField = (
		<FormSelectField
			name='analysis_userContributionType'
			label='User Contribution Type'
			control={formContext.control}
			errorField={errors.analysis_userContributionType}
			defaultOption='percentage'
			options={[
				{value: 'percentage', label: 'Paycheck Percentage'},
				{value: 'fixed', label: 'Fixed Amount'},
			]}
			disable={false}
			tooltip={
				<>
					<Typography variant='inherit'>
						The type of user contribution to the vehicle.
					</Typography>
					<Typography variant='inherit'>
						Percentage calculates the contribution based on a percentage of salary.
					</Typography>
					<Typography variant='inherit'>
						Fixed calculates the contribution based on a fixed amount.
					</Typography>
				</>
			}
		/>
	);

	const userContributionField = React.useMemo(() => {
		switch (userContributionType) {
			case 'percentage':
				return (
					<FormTextField
						name='analysis_userContributionPercentage'
						label='User Contribution'
						control={formContext.control}
						errorField={errors.analysis_userContributionPercentage}
						tooltip='The percentage of the a paycheck used to contribute to a vehicle.'
					/>
				);
			case 'fixed':
				return (
					<FormTextField
						name='analysis_userContributionAmount'
						label='User Contribution'
						control={formContext.control}
						errorField={errors.analysis_userContributionAmount}
						tooltip='The amount of money contributed to the vehicle each month.'
						decoration='currency'
					/>
				);
			default:
				return null;
		}
	}, [userContributionType, formContext.control, errors]);

	return (
		<>
			<Grid item xs={6} sm={4}>
				{salaryField}
			</Grid>
			<Grid item xs={6} sm={4}>
				{payFrequencyField}
			</Grid>
			<Grid item xs={0} sm={4} />
			<Grid item xs={6} sm={4}>
				{maxEmployerContributionField}
			</Grid>
			<Grid item xs={6} sm={4}>
				{employerMatchField}
			</Grid>
			<Grid item xs={0} sm={4} />
			<Grid item xs={6} sm={4}>
				{userContributionTypeField}
			</Grid>
			<Grid item xs={6} sm={4}>
				{userContributionField}
			</Grid>
		</>
	);
};
