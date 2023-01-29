import React from 'react';
import {useFormContext} from 'react-hook-form';

export const BondForm = () => {
	const formContext = useFormContext();
	const [showForm, setShowForm] = React.useState(true);


	React.useEffect(() => {
		setShowForm(true);
		return () => {
			formContext.unregister('bondName');
			setShowForm(false);
		};
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, []);


	return (<>
		{showForm && (
			<div>
				<label>Name</label>
				<input {...formContext.register("bondName")} /><br />
			</div>)
		}
	</>);
}


