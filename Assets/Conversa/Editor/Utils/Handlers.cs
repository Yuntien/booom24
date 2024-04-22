using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Conversa.Editor
{
    public static class Handlers
    {

        private static TypeCache.MethodCollection GetEditPropertyHandlers() =>
            TypeCache.GetMethodsWithAttribute<EditPropertyHandlerAttribute>();

        private static TypeCache.MethodCollection GetDeletePropertyHandlers() =>
            TypeCache.GetMethodsWithAttribute<DeletePropertyHandlerAttribute>();

        private static TypeCache.MethodCollection GetDeleteSelectionHandlers() =>
            TypeCache.GetMethodsWithAttribute<DeleteSelectionHandlerAttribute>();

        public static void ExecuteEditProperty(object[] args)
        {
            foreach (var handler in GetEditPropertyHandlers())
                handler.Invoke(null, args);
        }

        public static void ExecuteDeleteProperty(object[] args)
        {
            foreach (var handler in GetDeletePropertyHandlers())
                handler.Invoke(null, args);
        }

        public static void ExecuteDeleteNodes(object[] args)
        {
            foreach (var handler in GetDeleteSelectionHandlers())
                handler.Invoke(null, args);
        }

        public static string HandleSerializeGraphElements(IEnumerable<GraphElement> elements)
        {
            Debug.LogWarning(
                "Conversa: Sorry, but this function is not yet implemented. Please join Discord if you have any questions.");
            return "";
        }

        public static void HandleUnserializeAndPaste(string operationName, string data)
        {
            Debug.LogWarning(
                "Conversa: Sorry, but this function is not yet implemented. Please join Discord if you have any questions.");
        }

        public static bool HandleCanPasteSerializedData(string data) => false;
    }
}