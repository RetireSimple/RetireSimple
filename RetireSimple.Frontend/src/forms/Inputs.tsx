import {FormControl, FormHelperText, InputLabel, MenuItem, Select, TextField} from '@mui/material';
import React from 'react'
import {Control, Controller} from 'react-hook-form';

export interface FormTextFieldProps {
	name: string;
	label:string;
	control: Control
	errorField: any;
}

export const FormTextField = (props: FormTextFieldProps) => {
	const memo = React.useMemo(() => (
		<Controller
			name={props.name}
			control={props.control}
			defaultValue={''}
			render={({field}) => (
				<TextField {...field}
					label={props.label}
					fullWidth
					size='small'
					error={!!props.errorField}
					helperText={props.errorField?.message as string} />
			)} />), [props.name, props.label, props.control, props.errorField]);

	return (memo)
}

export interface FormSelectFieldProps {
	name: string;
	label:string;
	options: {value:string, label:string}[],
	defaultOption: string;	//Assert it is in options for easy programming
	control: Control
	errorField: any;
}

export const FormSelectField = (props: FormSelectFieldProps) => {
	const memo = React.useMemo(() => (
		<Controller
			name={props.name}
			control={props.control}
			defaultValue={props.defaultOption}
			render={({field}) => (
				<FormControl fullWidth
					error={!!props.errorField}
					size='small'
				>
					<InputLabel id={props.name}>{props.label}</InputLabel>
					<Select {...field} label={props.name}>
						{props.options.map((option) => (
							<MenuItem
								key={option.value}
								value={option.value}>
								{option.label}
							</MenuItem>
						))}
					</Select>
					<FormHelperText>
						{props.errorField?.message as string}
					</FormHelperText>
				</FormControl>
			)} />),
	[props.name, props.label, props.control, props.errorField, props.options, props.defaultOption]);

	return (memo)
}
