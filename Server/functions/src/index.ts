/**
 * Import function triggers from their respective submodules:
 *
 * import {onCall} from "firebase-functions/v2/https";
 * import {onDocumentWritten} from "firebase-functions/v2/firestore";
 *
 * See a full list of supported triggers at https://firebase.google.com/docs/functions
 */

import { onRequest } from "firebase-functions/v2/https";
import {getFirestore} from "firebase-admin/firestore";
import * as logger from "firebase-functions/logger";
import {initializeApp} from "firebase-admin/app";

// Start writing functions
// https://firebase.google.com/docs/functions/typescript

initializeApp();

exports.logRun = onRequest(async (req, res) => {
    const runData = JSON.parse(req.body);

    const writeResult = await getFirestore().collection("messages").add(runData);
    const message = `Message with ID: ${writeResult.id} was added.`;
    logger.log(message);
    res.json({result: message});
});
