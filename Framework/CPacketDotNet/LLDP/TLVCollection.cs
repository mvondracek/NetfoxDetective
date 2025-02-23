using System.Collections.ObjectModel;
using System.Reflection;
using log4net;

namespace PacketDotNet.LLDP
{
    /// <summary>
    ///     Custom collection for TLV types
    ///     Special behavior includes:
    ///     - Preventing an EndOfLLDPDU tlv from being added out of place
    ///     - Checking and throwing exceptions if one-per-LLDP packet TLVs are added multiple times
    /// </summary>
    public class TLVCollection : Collection<TLV>
    {
#if DEBUG
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
#else
    // NOTE: No need to warn about lack of use, the compiler won't
    //       put any calls to 'log' here but we need 'log' to exist to compile
#pragma warning disable 0169, 0649
        private static readonly ILogInactive log;
#pragma warning restore 0169, 0649
#endif

        /// <summary>
        ///     Override to:
        ///     - Prevent duplicate end tlvs from being added
        ///     - Ensure that an end tlv is present
        ///     - Replace any automatically added end tlvs with the user provided tlv
        /// </summary>
        /// <param name="index">
        ///     A <see cref="System.Int32" />
        /// </param>
        /// <param name="item">
        ///     A <see cref="TLV" />
        /// </param>
        protected override void InsertItem(int index, TLV item)
        {
            log.DebugFormat("index {0}, TLV.GetType {1}, TLV.Type {2}", index, item.GetType(), item.Type);

            // if this is the first item and it isn't an End TLV we should add the end tlv
            if((this.Count == 0) && (item.Type != TLVTypes.EndOfLLDPU))
            {
                log.Debug("Inserting EndOfLLDPDU");
                base.InsertItem(0, new EndOfLLDPDU());
            }
            else if(this.Count != 0)
            {
                // if the user is adding their own End tlv we should replace ours
                // with theirs
                if(item.Type == TLVTypes.EndOfLLDPU)
                {
                    log.DebugFormat("Replacing {0} with user provided {1}, Type {2}", this[this.Count - 1].GetType(), item.GetType(), item.Type);
                    this.SetItem(this.Count - 1, item);
                    return;
                }
            }

            // if we have no items insert the first item wherever
            // if we have items insert the item befor the last item as the last item is a EndOfLLDPDU
            var insertPosition = (this.Count == 0)? 0 : this.Count - 1;

            log.DebugFormat("Inserting item at position {0}", insertPosition);

            base.InsertItem(insertPosition, item);
        }
    }
}