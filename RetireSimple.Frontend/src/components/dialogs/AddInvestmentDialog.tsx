import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Button, Dialog, DialogActions, DialogTitle} from '@mui/material';
import {FieldValues, FormProvider, useForm} from 'react-hook-form';
import {Form, useNavigate, useSubmit} from 'react-router-dom';
import {investmentFormSchema} from '../../data/FormSchema';
import {InvestmentDataForm} from '../../forms/InvestmentDataForm';
import {addStock} from '../../api/InvestmentApi';
import React from 'react';

// export interface AddInvestmentDialogProps {
// 	onClose: () => void;
// }

export const AddInvestmentDialog = () => {
	const formContext = useForm({
		shouldUnregister: true,
		resolver: yupResolver(investmentFormSchema)});

	const navigate = useNavigate();
	const submit = useSubmit();

	const handleClose = () => {
		navigate(-1);
	};

	const formRef: React.MutableRefObject<any> = React.useRef(null);

	const handleAdd = (data:FieldValues) => {
		console.log(data);
		//force all fields to be strings
		data = Object.fromEntries(
			Object.entries(data).map(([key, value]) => [key, value.toString()]));

		addStock(data).then((res) => {
			submit(null, {method: 'post', action: '/add'})
			handleClose();
		});
	};

	const triggerSubmit= ()=>{
		if(formRef.current){
			formRef.current.dispatchEvent(
				new Event('submit', {cancelable: true, bubbles: true}));
		}
	}

	return (
		<FormProvider {...formContext}>
			<Dialog open
				maxWidth='md'>
				<DialogTitle>Add Investment</DialogTitle>
				<Form onSubmit={formContext.handleSubmit(handleAdd, (errors)=>console.log(errors))}>
					<Box sx={{padding: '2rem'}}>
						<InvestmentDataForm />
					</Box>
					<DialogActions>
						<Button onClick={handleClose}>Cancel</Button>
						<Button type='submit' onClick={triggerSubmit}>Add</Button>
					</DialogActions>
				</Form>

			</Dialog>
		</FormProvider>
	);
};
