import type { Actions } from './$types';
import { env } from '$env/dynamic/private';
import oracledb from 'oracledb';
import { fail } from '@sveltejs/kit';

export const actions: Actions = {
    addParcelReceipt: async (event: any) => {
        const data = await event.request.formData();
        console.log(data);
        const receiver = data.get('receiver');
        const receiptLocation = data.get('receiptLocation');
        const carrier = data.get('carrier');
        const routeLocation = data.get('routeLocation');
        const trackNumber = data.get('trackNumber');
        let connection;

        try {
            connection = await oracledb.getConnection({ user: env.DBUSER, password: env.DBUSERPASS, connectionString: env.DB });

            console.log("Successfully connected to Oracle Database");

            const sql = (`INSERT INTO ParcelReceipt (RECEIVER_ID, RECEIPT_LOCATION, CARRIER, TRACKING_NUMBER, ROUTING_LOCATION) VALUES (:1, :2, :3, :4, :5)`);

            await connection.execute(sql, [receiver, receiptLocation, carrier, trackNumber, routeLocation])

            connection.commit();

            return { success: true };

        } catch (err) {
            console.log(err);
            return fail(400, { message: "Cannot connect to db!" });
        }
    }
}